using Dlog.Api.Utils;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dlog.Api.Models
{
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
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }

    }
}
