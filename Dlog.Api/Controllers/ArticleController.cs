using Dlog.Api.Data;
using Dlog.Api.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dlog.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ArticleController : BlogControllerBase
    {
        public ArticleController(ICache cache, IDatabase database) : base(cache, database) { }
        public ResponseModel Get(string id)
        {
            cache.AddSeen(id);
            var serverArticle = database.GetArticleByID(id);
            var article = new ArticleModel()
            {
                Content = serverArticle.Content,
                Date = serverArticle.Date,
                Seen = serverArticle.Seen,
                Tags = serverArticle.Tags,
                Title = serverArticle.Title,
                Summary = serverArticle.Summary
            };
            return new ResponseModel()
            {
                Article = article
            };
        }
    }
}
