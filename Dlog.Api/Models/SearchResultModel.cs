using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dlog.Api.Models
{
    public class SearchResultModel
    {
        public string ID { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Summary { get; set; }
        public List<string> Tags { get; set; }
        public string Date { get; set; }
    }
}
