namespace Services.First.Utils
{
    public class RedisConfig
    {
        public RedisConfig(IConfiguration configuration)
        {
            Url = configuration["RedisConfig:Url"];
        }
        public string Url { get; set; }
    }
}
