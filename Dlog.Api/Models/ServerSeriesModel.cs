using Dlog.Api.Utils;
using MongoDB.Bson.Serialization.Attributes;
using Nest;
using System.Collections.Generic;

namespace Dlog.Api.Models
{
    [ESIndexName("seriesIndex")]
    [BsonIgnoreExtraElements]
    [ElasticsearchType(IdProperty = nameof(Name))]
    public class ServerSeriesModel
    {
        [MongoKey]
        public string Name { get; set; }
        public List<string> Articles { get; set; }
    }
}