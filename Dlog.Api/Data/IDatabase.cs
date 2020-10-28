using Dlog.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dlog.Api.Data
{
    public interface IDatabase
    {
        void UpdateArticles(List<ServerArticleModel> articles);
        void UpdateSeries(List<ServerSeriesModel> seriesModels);
        ServerArticleModel GetArticleByID(string articleID);
        List<ServerArticleModel> GetLatestArticles();
        List<ServerArticleModel> GetIndexArticles();
        Dictionary<string, List<ServerArticleModel>> GetNavTimeLine();
        List<string> GetTimeLineNodes();
        List<string> GetTags();
        List<TimeLineNodeModel> GetTimeLine(int? year);
        List<ServerArticleModel> GetByTag(string tag);
        List<SeriesModel> GetAllSeries();
        SeriesModel GetCurrentSeries(string articleID);
    }
}
