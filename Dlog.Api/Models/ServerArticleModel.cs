using Dlog.Api.Utils;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dlog.Api.Models
{
    [ESIndexName("blogIndex")]
    [BsonIgnoreExtraElements]
    [ElasticsearchType(IdProperty = nameof(ID))]
    public class ServerArticleModel
    {
        [MongoKey]
        public string ID { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Summary { get; set; }
        public long Seen { get; set; }
        public List<string> Tags { get; set; }
        public string Date { get; set; }
        public string Cover { get; set; }

    }
}
