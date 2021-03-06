﻿using CSRedis;
using DnsClient.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dlog.Api.Data
{
    public class RedisCache : ICache
    {
        const string ARTICLE_LIST = "ARTICLELIST";
        const string ARTICLE_SEEN = "SEEN";
        const string GENERATE_WORDS = "WORDS";
        private readonly CSRedisClient redisDB;

        public RedisCache(IConfiguration configuration, ILogger<RedisCache> logger)
        {
            var redis = configuration.GetSection("Redis");
            var redisContr = $"{redis["Address"]},defaultDatabase={redis["DefaultDatabase"]},password={redis["Password"]}";
            logger.LogInformation(redisContr);
            redisDB = new CSRedisClient(redisContr);
            RedisHelper.Initialization(redisDB);
        }
        public void SetArticleList(List<string> articles)
        {
            redisDB.Del(ARTICLE_LIST);
            redisDB.SAdd(ARTICLE_LIST, articles.ToArray());
        }
        public List<string> GetArticleList()
        {
            return redisDB.SMembers(ARTICLE_LIST).ToList();
        }

        public void AddSeen(string article)
        {
            redisDB.HIncrBy(ARTICLE_SEEN, article);
        }

        public long GetSeen(string article)
        {
            return redisDB.HGet<long>(ARTICLE_SEEN, article);
        }

        public Dictionary<string, long> GetAllSeen()
        {
            return redisDB.HGetAll<long>(ARTICLE_SEEN);
        }

        public Task<string[]> GetWordsAsync()
        {
            return redisDB.SMembersAsync(GENERATE_WORDS);
        }
    }
}
