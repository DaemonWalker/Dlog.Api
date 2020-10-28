using System.Collections.Generic;

namespace Dlog.Api.Models
{
    public class SeriesModel
    {
        public string Name { get; set; }
        public List<ArticleSummaryModel> Articles { get; set; }
    }
}