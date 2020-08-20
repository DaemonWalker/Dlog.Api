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
    public class BackgroundController : ControllerBase
    {
        private readonly BlogFetch blogFetch;
        public BackgroundController(BlogFetch blogFetch)
        {
            this.blogFetch = blogFetch;
        }

        [HttpPost]
        public IActionResult Fetch([FromBody] string password)
        {
            var env = Environment.GetEnvironmentVariable("fetchPwd");
            if (string.IsNullOrWhiteSpace(env) == false)
            {
                if (password != env)
                {
                    return Unauthorized();
                }
            }
            blogFetch.FetchBlogs();
            return Ok();
        }
    }
}
