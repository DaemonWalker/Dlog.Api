using Dlog.Api.Models;
using Dlog.Api.Utils;
using Microsoft.Extensions.Configuration;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dlog.Api.Data
{
    public class ESClient : ISearch
    {
        private readonly IConfiguration configuration;
        public ESClient(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public void AddArticleData(ServerArticleModel articleModel)
        {
            var client = GetIndex<ServerArticleModel>(out var idx);
            client.Index(articleModel, p => p.Index(idx).Id(articleModel.ID));
        }
        public void AddSeriesData(ServerSeriesModel seriesModel)
        {
            var client = GetIndex<ServerSeriesModel>(out var index);
            client.Index(seriesModel, p => p.Index(index).Id(seriesModel.Name));
        }

        public List<ServerArticleModel> Search(string text)
        {
            var results = GetIndex<ServerArticleModel>(out var _).Search<ServerArticleModel>(
                selector => selector.Query(
                    query => query.Bool(
                        b => b.Should(
                            mu => mu.Match(m => m.Field(f => f.Content).Query(text)),
                            mu => mu.Match(m => m.Field(f => f.Title).Query(text)),
                            mu => mu.Match(m => m.Field(f => f.Tags).Query(text))))))
                .Documents
                .Select(p => p)
                .ToList();
            return results;
        }

        private ElasticClient GetIndex<T>(out string index)
        {
            var es = configuration.GetSection("ES");
            index = ((ESIndexNameAttribute)typeof(T)
               .GetCustomAttributes(false)
               .First(predicate => predicate.GetType() == typeof(T)))
               .Name;
            var conn = new ConnectionSettings(new Uri(es["url"]))
                .DefaultIndex(index).DefaultMappingFor<ServerArticleModel>(m => m.IdProperty(p => p.ID));
            return new ElasticClient(conn);
        }
    }
}
