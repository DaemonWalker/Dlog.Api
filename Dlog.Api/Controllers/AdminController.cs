using Dlog.Api.BackgroundTasks;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dlog.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AdminController : ControllerBase
    {
        private readonly BlogFetch blogFetch;
        private readonly string adminPassword;
        public AdminController(BlogFetch blogFetch)
        {
            this.blogFetch = blogFetch;
            adminPassword = Environment.GetEnvironmentVariable("fetchPwd");
        }

        [HttpPost]
        public IActionResult UpdateArticles([FromForm] string password)
        {
            if (password != adminPassword)
            {
                return Unauthorized("错误的密码");
            }
            blogFetch.FetchBlogs();
            return Ok();
        }

        [HttpPost]
        public IActionResult UpdateSeries([FromForm] string password)
        {
            if (password != adminPassword)
            {
                return Unauthorized("错误的密码");
            }
            return Ok();
        }
    }
}
