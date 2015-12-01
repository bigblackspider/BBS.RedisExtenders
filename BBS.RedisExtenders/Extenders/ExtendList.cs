using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace BBS.RedisExtenders.Extenders
{
    public static class ExtendList
    {
        private static ConnectionMultiplexer _redis;

        public static ConnectionMultiplexer Redis => _redis ?? (_redis = ConnectionMultiplexer.Connect("localhost"));

        public static void RedisConnect<T>(this List<T> list, string configuration)
        {
            _redis?.Close();
            _redis = ConnectionMultiplexer.Connect(configuration);
        }

        public static void RedisGetAll<T>(this List<T> list)
        {
            if (list != null)
            {
                var db = Redis.GetDatabase();
                var setName = typeof(T).Name;
                list.AddRange(db.SetMembers(setName).Select(o => JsonConvert.DeserializeObject<T>(o)));
            }   
        }

        public static void RedisAdd<T>(this List<T> list, T item)
        {
            var db = Redis.GetDatabase();
            var setName = typeof (T).Name;
            db.SetAdd(setName, JsonConvert.SerializeObject(item));
            list.Add(item);
        }

        public static void RedisUpdate<T>(this List<T> list)
        {
            var db = Redis.GetDatabase();
            var setName = typeof (T).Name;
            foreach (var member in db.SetMembers(setName))
                db.SetRemove(setName, member);
            foreach (var item in list)
                db.SetAdd(setName, JsonConvert.SerializeObject(item));
        }

        public static bool RedisRemove<T>(this List<T> list, T item)
        {
            var db = Redis.GetDatabase();
            var setName = typeof (T).Name;
            db.SetRemove(setName, JsonConvert.SerializeObject(item));
            return list.Remove(item);
        }

        public static void RedisClear<T>(this List<T> list)
        {
            var db = Redis.GetDatabase();
            var setName = typeof (T).Name;
            foreach (var member in db.SetMembers(setName))
                db.SetRemove(setName, member);
            list.Clear();
        }

        public static long RedisNextId<T>(this List<T> list)
        {
            try
            {
                var db = Redis.GetDatabase();
                var keyName = typeof(T).Name + "Key";
                return db.HashIncrement(keyName, 1);
            }
            catch (Exception)
            {
                return -1;
            }
        }
    }
}