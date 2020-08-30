using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace Dlog.Api.Middlewares.ServerResponse.Models
{
    public class ResponseModel
    {
        public ServerResponseInfoModel ServerResponse { get; set; } = new ServerResponseInfoModel();
       
        public NavDataModel NavData { get; set; }
        public List<ArticleSummaryModel> ArticleSummaries { get; set; }
        public string Token { get; set; }
        public List<TimeLineNodeModel> TimeLine { get; set; }
        public ArticleModel Article { get; set; }
        public List<SearchResultModel> SearchResult { get; set; }
    }
}
