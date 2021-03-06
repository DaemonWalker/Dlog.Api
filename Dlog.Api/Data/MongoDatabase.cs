﻿using Dlog.Api.Models;
using Dlog.Api.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dlog.Api.Data
{
    public class MongoDatabase : IDatabase
    {
        private readonly IMongoDatabase mongoDB;

        public MongoDatabase(IConfiguration configuration, ILogger<MongoDatabase> logger)
        {
            var mongo = configuration.GetSection("MongoDB");
            var mongoContr = $"mongodb://{mongo["Account"]}:{mongo["Password"]}@{mongo["Address"]}/?authSource={mongo["AuthSource"]}";
            logger.LogInformation(mongoContr);
            mongoDB = new MongoClient(mongoContr).GetDatabase("blog");
        }

        public void UpdateArticles(List<ServerArticleModel> articles)
        {
            var articleDB = mongoDB.GetArticle();
            foreach (var article in articles)
            {
                articleDB.UpdateOne(
                    article.GetMongoKey(),
                    article.ToMongoUpdate(),
                    options: new UpdateOptions() { IsUpsert = true });
            }
        }

        public void UpdateSeries(List<ServerSeriesModel> seriesModels)
        {
            var seriesDB = mongoDB.GetSeries();
            foreach (var item in seriesModels)
            {
                seriesDB.UpdateOne(
                    item.GetMongoKey(),
                    item.ToMongoUpdate(),
                    options: new UpdateOptions() { IsUpsert = true });
            }
        }

        public ServerArticleModel GetArticleByID(string articleID)
        {
            var articles = mongoDB.GetArticle();

            try
            {
                var result = articles.Find(new ServerArticleModel() { ID = articleID }.GetMongoKey()).First();
                return result;
            }
            catch
            {
                return new ServerArticleModel();
            }
        }
        public List<ServerArticleModel> GetLatestArticles()
        {
            var articles = mongoDB.GetArticle();

            return articles.Find(Builders<ServerArticleModel>.Filter.Gt(
              p => p.Date, DateTime.Now.AddMonths(-3).ToString("yyyy-MM-dd"))).SortByDescending(p => p.Date).ToList();
        }
        public List<ServerArticleModel> GetIndexArticles()
        {
            var articles = mongoDB.GetArticle();

            return articles.Find(Builders<ServerArticleModel>.Filter.Empty).SortByDescending(p => p.Date).ToList().ToList();
        }
        public Dictionary<string, List<ServerArticleModel>> GetNavTimeLine()
        {
            return mongoDB.GetArticle().Find(Builders<ServerArticleModel>.Filter.Empty).ToList().ToDictionaryNew(p => p.Date.Substring(0, 7));
        }
        public List<string> GetTimeLineNodes()
        {
            var projection = Builders<ServerArticleModel>.Projection.Expression<string>(p => p.Date.Substring(0, 4));
            return mongoDB
                .GetArticle()
                .Find(Builders<ServerArticleModel>.Filter.Empty)
                .Project(projection)
                .ToList()
                .Distinct()
                .ToList();
        }
        public List<string> GetTags()
        {
            var projection = Builders<ServerArticleModel>.Projection.Expression(p => p.Tags);
            var tags = mongoDB.GetArticle().Find(Builders<ServerArticleModel>.Filter.Empty).Project(projection).ToList();
            var list = new List<string>();
            foreach (var tag in tags)
            {
                list.AddRange(tag);
            }
            return list.Distinct().OrderBy(p => p).ToList();
        }
        public List<TimeLineNodeModel> GetTimeLine(int? year)
        {
            var regex = new BsonRegularExpression($"{year}");
            var filter = Builders<ServerArticleModel>.Filter.Empty;
            if (year != null)
            {
                filter = Builders<ServerArticleModel>.Filter.Regex(p => p.Date, regex);
            }
            return mongoDB
                .GetArticle()
                .Find(filter)
                .SortByDescending(p => p.Date)
                .ToList()
                .Select(p => new TimeLineNodeModel()
                {
                    BlogDate = p.Date,
                    Title = p.Title,
                    Url = p.ID
                })
                .ToList();
        }
        public List<ServerArticleModel> GetByTag(string tag)
        {
            var filter = Builders<ServerArticleModel>.Filter.All(p => p.Tags, new List<string>() { tag });
            return mongoDB.GetArticle().Find(filter).ToList();
        }

        public List<SeriesModel> GetAllSeries()
        {
            var serieses = mongoDB.GetSeries();
            return serieses.Find(Builders<ServerSeriesModel>.Filter.Empty)
                .ToList()
                .Select(p => new SeriesModel()
                {
                    Name = p.Name,
                    Articles = p.Articles.Select(q => this.GetArticleByID(q).ToSummary()).ToList()
                })
                .ToList();
        }

        public SeriesModel GetCurrentSeries(string currentID)
        {
            var serieses = mongoDB.GetSeries();
            var filter = Builders<ServerSeriesModel>.Filter.All(p => p.Articles, new List<string>() { currentID });
            var serverSeries = serieses.Find(filter).FirstOrDefault();
            if (serverSeries == default)
            {
                return default;
            }
            return new SeriesModel()
            {
                Name = serverSeries.Name,
                Articles = serverSeries.Articles.Select(q => this.GetArticleByID(q).ToSummary()).ToList()
            };
        }

    }
}
