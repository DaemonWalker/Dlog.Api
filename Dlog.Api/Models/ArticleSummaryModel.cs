using Dlog.Api.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dlog.Api.Models
{
    public class ArticleSummaryModel
    {
        public string Title { get; set; }
        public string Summary { get; set; }
        public string ImgPath { get; set; }
        public string Url { get; set; }
        public List<string> Tags { get; set; }
        public long Seen { get; set; }
        public int Likes { get; set; }
        public int Comments { get; set; }
    }
}
