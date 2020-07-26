using Dlog.Api.Data;
using Dlog.Api.Models;
using Dlog.Api.Utils;
using Hangfire;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dlog.Api.BackgroundTasks
{
    public class BlogFetch : JobActivator
    {
        const string CONTENTNAME = @"content.md";
        const string TAGSNAME = @"tags";
        const string SUMMARYNAME = @"summary";
        const string TITLENAME = @"title";
        const string DATENAME = @"date";
        readonly Encoding contentEncoding = Encoding.UTF8;
        private readonly IDatabase database;
        private readonly ISearch search;
        public BlogFetch(IDatabase database,ISearch search)
        {
            this.database = database;
            this.search = search;
        }
        public void FetchBlogs()
        {
            var blogDir = new DirectoryInfo(Constance.BLOGDIRNAME);
            var articleLists = new List<ServerArticleModel>();
            foreach (var dir in blogDir.GetDirectories())
            {
                try
                {
                    var article = new ServerArticleModel() { ID = dir.Name };
                    var files = dir.GetFiles();
                    article.Content = File.ReadAllText(files.First(p => p.Name == CONTENTNAME).FullName, contentEncoding);
                    article.Tags = File.ReadAllLines(files.First(p => p.Name == TAGSNAME).FullName, contentEncoding).ToList();
                    article.Summary = File.ReadAllText(files.First(p => p.Name == SUMMARYNAME).FullName, contentEncoding);
                    article.Seen = 0;
                    article.Title = File.ReadAllText(files.First(p => p.Name == TITLENAME).FullName, contentEncoding);
                    article.Date = File.ReadAllText(files.First(p => p.Name == DATENAME).FullName, contentEncoding);
                    articleLists.Add(article);

                    search.AddArticleData(article);
                }
                catch (Exception e)
                {
                }
            }
            database.UpdateArticles(articleLists);
        }
    }
}
