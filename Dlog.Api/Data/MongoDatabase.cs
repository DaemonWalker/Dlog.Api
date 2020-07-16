﻿using Dlog.Api.Models;
using Dlog.Api.Utils;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dlog.Api.Data
{
    public class MongoDatabase : IDatabase
    {
        private readonly IMongoDatabase mongoDB;

        public MongoDatabase(IConfiguration configuration)
        {
            var mongo = configuration.GetSection("MongoDB");
            var mongoContr = $"mongodb://{mongo["Account"]}:{mongo["Password"]}@{mongo["Address"]}/?authSource={mongo["AuthSource"]}";
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
              p => p.Date, DateTime.Now.AddMonths(-3).ToString("yyyy-MM-dd"))).ToList();
        }
        public List<ServerArticleModel> GetIndexArticles()
        {
            var articles = mongoDB.GetArticle();

            return articles.Find(Builders<ServerArticleModel>.Filter.Empty).Limit(10).SortBy(p => p.Date).ToList().ToList();
        }
        public Dictionary<string, List<ServerArticleModel>> GetNavTimeLine()
        {
            return mongoDB.GetArticle().Find(Builders<ServerArticleModel>.Filter.Empty).ToList().ToDictionaryNew(p => p.Date.Substring(0, 7));
        }
        public List<string> GetTimeLineNodes()
        {
            var projection = Builders<ServerArticleModel>.Projection.Expression<string>(p => p.Date.Substring(0, 7));
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
            return list;
        }
    }
}
