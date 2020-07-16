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
                IndexArticles = summaries
            };
        }

        [HttpGet]
        public ResponseModel GetTimeLine(int? year)
        {
            var timeLine = new List<TimeLineNodeModel>()
            {
                new TimeLineNodeModel()
                {
                     BlogDate="2020-06-20",
                     Title="测试文章",
                     Url="this-is-an-test-article"
                },
                new TimeLineNodeModel()
                {
                     BlogDate="2020-04-10",
                     Title="测试文章2",
                     Url="this-is-an-test-article-2"
                },
                new TimeLineNodeModel()
                {
                     BlogDate="2020-03-30",
                     Title="测试文章3",
                     Url="this-is-an-test-article-3"
                }
            };
            if (year == null)
            {
                timeLine.Add(new TimeLineNodeModel()
                {
                    BlogDate = "2019-10-16",
                    Title = "又老了一岁",
                    Url = "another-birthday"
                });
                timeLine.Add(new TimeLineNodeModel()
                {
                    BlogDate = "2018-10-16",
                    Title = "Age++",
                    Url = "birthday-2018"
                });
            }
            return new ResponseModel()
            {
                ServerResponse = new ServerResponseInfoModel(),
                TimeLine = timeLine.OrderByDescending(p => p.BlogDate).ToList()
            };
        }
    }
}
