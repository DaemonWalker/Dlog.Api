using Dlog.Api.Data;
using Dlog.Api.Models;
using Dlog.Api.Utils;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dlog.Api.Controllers
{
    [EnableCors]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class SummaryController : BlogControllerBase
    {
        public SummaryController(ICache cache, IDatabase database) : base(cache, database) { }
        [HttpGet]
        public ResponseModel GetNavData()
        {
            var recents = database.GetLatestArticles().Select(p => p.ToLink()).ToList();
            var timeline = database.GetTimeLineNodes().Select(p => new LinkModel()
            {
                Content = p,
                Url = p.Replace("-", "")
            }).ToList();
            var tags = database.GetTags().Select(p => new LinkModel()
            {
                Content = p,
                Url = p
            }).ToList();
            var navData = new NavDataModel()
            {
                Recents = recents,
                TimeLine = timeline,
                Tags = tags
            };

            return new ResponseModel()
            {
                NavData = navData,
                ServerResponse = new ServerResponseInfoModel()
            };
        }

        [HttpGet]
        public ResponseModel GetIndexList()
        {
            var seens = cache.GetAllSeen();
            var serverArticles = database.GetIndexArticles();
            var summaries = new List<ArticleSummaryModel>();
            foreach (var article in serverArticles)
            {
                var summary = article.ToSummary();
                if (seens.TryGetValue(article.ID, out var seen))
                {
                    summary.Seen = seen;
                }
                summaries.Add(summary);
            }
            return new ResponseModel()
            {
                ServerResponse = new ServerResponseInfoModel(),
                ArticleSummaries = summaries
            };
        }

        [HttpGet]
        public ResponseModel GetTimeLine(int? year)
        {
            return new ResponseModel()
            {
                ServerResponse = new ServerResponseInfoModel(),
                TimeLine = database.GetTimeLine(year)
            };
        }

        [HttpGet]
        [Route("{tagId}")]
        public ResponseModel GetArticlesByTag(string tagId)
        {
            if (string.IsNullOrEmpty(tagId))
            {
                return new ResponseModel();
            }

            return new ResponseModel()
            {
                ArticleSummaries = database.GetByTag(tagId).Select(p => p.ToSummary()).ToList()
            };
        }
    }
}
