using Dlog.Api.Middlewares.ServerResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dlog.Api.Models
{
    [ResponsePropertyName("Article")]
    public class ArticleModel
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public List<string> Tags { get; set; }
        public string Date { get; set; }
        public long Seen { get; set; }
    }
}
