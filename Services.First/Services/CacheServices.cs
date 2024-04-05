using Services.First.Services.Abstraction;
using Services.First.Utils;
using StackExchange.Redis;
using Newtonsoft.Json;

namespace Services.First.Services
{
    public class CacheServices : ICacheServices
    {
        private readonly IDatabase _db;
        public CacheServices(RedisHelper redisHelper)
        {
            _db = redisHelper.Connection.GetDatabase();
        }

        public T GetData<T>(string key)
        {
            var value = _db.StringGet(key);
            if (!string.IsNullOrEmpty(value))
            {
                return JsonConvert.DeserializeObject<T>(value);
            }

            return default;
        }

        public bool RemoveData(string key)
        {
            bool _isKeyExist = _db.KeyExists(key);
            if (_isKeyExist == true)
            {
                return _db.KeyDelete(key);
            }
            return false;
        }

        public bool SetData<T>(string key, T value, DateTimeOffset expirationTime)
        {
            TimeSpan expiryTime = expirationTime.DateTime.Subtract(DateTime.Now);
            var isSet = _db.StringSet(key, JsonConvert.SerializeObject(value), expiryTime);

            return isSet;
        }
    }
}
