using Dlog.Api.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Dlog.Api.Midwares
{
    public class SitemapGenerator : BackgroundService
    {
        private readonly IDatabase database;
        private readonly ICache cache;
        private IServiceScope scope;

        const string HEAD = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n";
        const string URLSET_HEAD = "<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\">\n";
        const string URLSET_END = "</urlset>\n";
        const string URL_HEAD = "    <url>\n";
        const string URL_END = "    </url>\n";

        public const string SITEMAP_NAME = "sitemap.xml";
        public const string SITEMAPDIR = "sitemap";

        public SitemapGenerator(IServiceScopeFactory scopeFactory)
        {
            this.scope = scopeFactory.CreateScope();
            this.database = this.scope.ServiceProvider.GetService<IDatabase>();
            this.cache = this.scope.ServiceProvider.GetService<ICache>();
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.Factory.StartNew(async () =>
            {
                var date = DateTime.MinValue;
                while (true)
                {
                    if (date.Date != DateTime.Now.Date)
                    {
                        date = DateTime.Now;
                        if (Directory.Exists(SITEMAPDIR) == false)
                        {
                            Directory.CreateDirectory(SITEMAPDIR);
                        }
                        await GenerateSiteMap();
                    }
                    await Task.Delay(15);
                }
            }, TaskCreationOptions.LongRunning);
        }
        private async Task GenerateSiteMap()
        {
            var articles = database.GetIndexArticles()
                .Select(p => new Url { loc = $"https://www.daemonwow.com/article/{p.ID.ToString()}" })
                .ToList();

            var random = new Random();
            var words = await cache.GetWordsAsync();
            words = words.OrderBy(p => random.Next(words.Length)).ToArray();
            var titles = GenerateTitles(words, 6, 5000 + random.Next(1000));

            articles.AddRange(titles.Select(p => new Url { loc = $"https://www.daemonwow.com/diary/{string.Join("-", p)}" }));

            var fileName = Path.Combine(SITEMAPDIR, SITEMAP_NAME);
            File.Create(fileName).Close();
            await File.AppendAllTextAsync(fileName, HEAD);
            await File.AppendAllTextAsync(fileName, URLSET_HEAD);
            foreach (var article in articles)
            {
                await File.AppendAllTextAsync(fileName, URL_HEAD);
                foreach (var prop in typeof(Url).GetProperties())
                {
                    await File.AppendAllTextAsync(fileName,
                        $"        <{prop.Name}>{prop.GetValue(article)}</{prop.Name}>\n");
                }
                await File.AppendAllTextAsync(fileName, URL_END);
            }
            await File.AppendAllTextAsync(fileName, URLSET_END);

        }

        static List<List<string>> GenerateTitles(string[] words, int targetLength, int totalCount)
        {
            var result = new List<List<string>>(totalCount);
            var queue = new Queue<(int index, List<string> words)>();
            for (int i = 0; i <= words.Length - targetLength; i++)
            {
                queue.Enqueue((i, new List<string>(targetLength) { words[i] }));
            }
            while (result.Count < totalCount)
            {
                var item = queue.Dequeue();
                for (int i = item.index + 1; i < words.Length; i++)
                {
                    var list = new List<string>(item.words);
                    list.Add(words[i]);
                    if (list.Count == targetLength)
                    {
                        result.Add(list);
                        if (result.Count >= totalCount)
                        {
                            return result;
                        }
                    }
                    queue.Enqueue((i, list));
                }
            }
            return result;
        }
        static int GetTotalCount(int total, int pick)
        {
            long r = 1;
            for (int i = total; i > total - pick; i--)
            {
                r = r * i;
            }
            for (int i = pick; i > 1; i--)
            {
                r = r / i;
            }
            return (int)r;
        }
    }
    class Url
    {
        public string loc { get; set; }
        public string lastmod => DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss+08:00");
    }
}
