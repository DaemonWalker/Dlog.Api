using Dlog.Api.Data;
using Dlog.Api.Models;
using Dlog.Api.Utils;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dlog.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    public class SearchController: ControllerBase
    {
        private readonly ISearch search;
        public SearchController(ISearch search)
        {
            this.search = search;
        }
        [Route("{text}")]
        public List<ArticleModel> Get(string text)
        {
            var serverArticles= search.Search(text);
            return serverArticles.Select(p => p.ToArticle()).ToList();
        }
    }
}
