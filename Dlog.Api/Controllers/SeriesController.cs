using System.Collections.Generic;
using Dlog.Api.Data;
using Dlog.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Dlog.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class SeriesController : ControllerBase
    {
        private readonly IDatabase database;
        public SeriesController(IDatabase database)
        {
            this.database = database;
        }

        [HttpGet]
        public List<SeriesModel> GetAllSeries()
        {
            return this.database.GetAllSeries();
        }

        [HttpGet]
        public SeriesModel GetCurrentSeries(string articleID)
        {
            return this.database.GetCurrentSeries(articleID);
        }

    }
}