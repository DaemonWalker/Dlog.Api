using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dlog.Api.Data
{
    public interface ICache
    {
        Dictionary<string, long> GetAllSeen();
        long GetSeen(string article);
        void AddSeen(string article);
        void SetArticleList(List<string> articles);
        List<string> GetArticleList();
        Task<string[]> GetWordsAsync();
    }
}
