using Dlog.Api.Models;
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
        private readonly ElasticClient elasticClient;
        private readonly string esIndex;
        public ESClient(IConfiguration configuration)
        {
            var es = configuration.GetSection("ES");
            esIndex = es["index"];
            var conn = new ConnectionSettings(new Uri(es["url"])).DefaultIndex(esIndex).DefaultMappingFor<ServerArticleModel>(m => m.IdProperty(p => p.ID));
            elasticClient = new ElasticClient(conn);
        }
        public void AddArticleData(ServerArticleModel articleModel)
        {
            elasticClient.Index(articleModel, p => p.Index(esIndex).Id(articleModel.ID));
        }

        public List<ServerArticleModel> Search(string text)
        {
            var results = elasticClient.Search<ServerArticleModel>(
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
    }
}
