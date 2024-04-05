namespace Services.First.Utils
{
    public class MessageBrokerConfig
    {
        public MessageBrokerConfig(IConfiguration configuration)
        {
            HostName = configuration["MessageBrokerConfig:HostName"];
            UserName = configuration["MessageBrokerConfig:UserName"];
            Password = configuration["MessageBrokerConfig:Password"];
            VirtualHost = configuration["MessageBrokerConfig:VirtualHost"];
        }

        public string HostName { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string VirtualHost { get; set; }
    }
}
