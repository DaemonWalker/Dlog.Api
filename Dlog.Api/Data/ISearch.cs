using Dlog.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dlog.Api.Data
{
    public interface ISearch
    {
        void AddArticleData(ServerArticleModel articleModel);
        void AddSeriesData(ServerSeriesModel seriesModel);
        List<ServerArticleModel> Search(string text);
    }
}
