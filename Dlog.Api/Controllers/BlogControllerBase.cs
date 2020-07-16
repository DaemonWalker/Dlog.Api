using Dlog.Api.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dlog.Api.Controllers
{
    public class BlogControllerBase : ControllerBase
    {
        protected readonly ICache cache;
        protected readonly IDatabase database;
        public BlogControllerBase(ICache cache,IDatabase database)
        {
            this.cache = cache;
            this.database = database;
        }
    }
}
