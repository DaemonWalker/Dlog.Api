using Dlog.Api.Models;
using Microsoft.AspNetCore.Mvc.TagHelpers.Cache;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace Dlog.Api.Utils
{
    public static class Entenssions
    {


        public static ArticleSummaryModel ToSummary(this ServerArticleModel article)
        {
            return new ArticleSummaryModel()
            {
                Seen = article.Seen,
                Summary = article.Summary,
                Tags = article.Tags,
                Url = article.ID,
                Title = article.Title
            };
        }
        public static LinkModel ToLink(this ServerArticleModel articleModel)
        {
            return new LinkModel()
            {
                Content = articleModel.Title,
                Url = articleModel.ID
            };
        }

        public static Dictionary<TKey, List<TValue>> ToDictionaryNew<TKey, TValue>(this IEnumerable<TValue> values, Expression<Func<TValue, TKey>> keySelector)
        {
            var dictionary = new Dictionary<TKey, List<TValue>>();
            foreach (var item in values)
            {
                var key = keySelector.Compile().Invoke(item);
                if (dictionary.ContainsKey(key) == false)
                {
                    dictionary.Add(key, new List<TValue>());
                }
                dictionary[key].Add(item);
            }
            return dictionary;
        }

    }
}
