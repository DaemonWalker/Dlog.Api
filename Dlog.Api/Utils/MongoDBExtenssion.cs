using Dlog.Api.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Dlog.Api.Utils
{
    public static class MongoDBExtenssion
    {
        public static IMongoCollection<ServerArticleModel> GetArticle(this IMongoDatabase mongoDB)
        {
            return mongoDB.GetCollection<ServerArticleModel>("article");
        }
        public static List<KeyValuePair<string, object>> ToBasonFormat<T>(this T t)
        {
            return typeof(T)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Select(p => new KeyValuePair<string, object>(p.Name, p.GetValue(t)))
                .ToList();
        }

        public static List<KeyValuePair<string, object>> ToBasonFormat<T>(this T t, IEnumerable<string> props)
        {
            return typeof(T)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => props.Contains(p.Name))
                .Select(p => new KeyValuePair<string, object>(p.Name, p.GetValue(t)))
                .ToList();
        }

        public static UpdateDefinition<T> ToMongoUpdate<T>(this T t)
        {
            var props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            UpdateDefinition<T> update = Builders<T>.Update.Set(props.First().Name, props.First().GetValue(t));

            for (int i = 1; i < props.Length; i++)
            {
                update = update.Set(props[i].Name, props[i].GetValue(t));
            }

            return update;
        }

        public static FilterDefinition<T> GetMongoKey<T>(this T t)
        {
            var key = typeof(T).GetProperties().Where(p => p.GetCustomAttribute(typeof(MongoKeyAttribute)) != null).ToList();
            if (key.Any() == false)
            {
                throw new Exception("No Mongo Key");
            }

            var filter = Builders<T>.Filter.Eq(key.First().Name, key.First().GetValue(t));
            for (int i = 1; i < key.Count; i++)
            {
                filter = filter & Builders<T>.Filter.Eq(key[i].Name, key[i].GetValue(t));
            }

            return filter;
        }

        public static T To<T>(this BsonValue bson)
        {
            var bsonValues = bson.ToBasonFormat();
            var props = typeof(T).GetProperties();
            var t = Activator.CreateInstance<T>();
            foreach (var prop in props)
            {
                if (bsonValues.Any(p => p.Key == prop.Name))
                {
                    prop.SetValue(t, bsonValues.First(p => p.Key == prop.Name).Value);
                }
            }
            return t;
        }

        public static T To<T>(this BsonDocument bson)
        {
            var bsonValues = bson.ToBasonFormat();
            var props = typeof(T).GetProperties();
            var t = Activator.CreateInstance<T>();
            foreach (var prop in props)
            {
                if (bsonValues.Any(p => p.Key == prop.Name))
                {
                    prop.SetValue(t, bsonValues.First(p => p.Key == prop.Name).Value);
                }
            }
            return t;
        }
    }
}
