using Dlog.Api.Data;
using Dlog.Api.Models;
using Dlog.Api.Utils;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Dlog.Api.BackgroundTasks
{
    public class BlogFetch
    {
        const string CONTENTNAME = @"content.md";
        const string INFONAME = @"info.json";
        private static readonly Encoding contentEncoding = Encoding.UTF8;
        private static readonly JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);
        private readonly IDatabase database;
        private readonly ISearch search;
        private ILogger<BlogFetch> logger;
        public BlogFetch(IDatabase database, ISearch search, ILogger<BlogFetch> logger)
        {
            this.database = database;
            this.search = search;
            this.logger = logger;
        }
        public void FetchBlogs()
        {
            var blogDir = new DirectoryInfo(Constance.BLOGDIRNAME);
            var articleLists = new List<ServerArticleModel>();
            foreach (var dir in blogDir.GetDirectories())
            {
                try
                {
                    var files = dir.GetFiles();
                    var json = File.ReadAllText(files.First(p => p.Name == INFONAME).FullName, contentEncoding);
                    var article = JsonSerializer.Deserialize<ServerArticleModel>(json, jsonSerializerOptions);
                    article.ID = dir.Name;
                    article.Content = File.ReadAllText(files.First(p => p.Name == CONTENTNAME).FullName, contentEncoding);
                    articleLists.Add(article);

                    search.AddArticleData(article);
                }
                catch (Exception e)
                {
                    logger.LogError(e.ToString());
                }
            }
            database.UpdateArticles(articleLists);
        }

        public void FetchSeries()
        {
            var seriesDir = new DirectoryInfo(Constance.SERIESNAME);
            var seriesList = new List<ServerSeriesModel>();
            foreach (var file in seriesDir.GetFiles())
            {
                var json = File.ReadAllText(file.FullName, contentEncoding);
                var series = JsonSerializer.Deserialize<ServerSeriesModel>(json, jsonSerializerOptions);

                seriesList.Add(series);
            }

            database.UpdateSeries(seriesList);
        }
    }
}
