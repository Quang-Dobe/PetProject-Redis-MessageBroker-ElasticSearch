using StackExchange.Redis;

namespace Services.First.Utils
{
    public class RedisHelper
    {
        private Lazy<ConnectionMultiplexer> _lazyConnection;

        public ConnectionMultiplexer Connection { get => _lazyConnection.Value; }

        public RedisHelper(RedisConfig config)
        {
            _lazyConnection = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(config.Url));
        }
    }
}
