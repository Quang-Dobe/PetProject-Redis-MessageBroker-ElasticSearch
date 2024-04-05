using RabbitMQ.Client;

namespace Services.First.Utils
{
    public class MessageBrokerHelper
    {
        private Lazy<ConnectionFactory> _lazyConnection;

        public ConnectionFactory Connection { get => _lazyConnection.Value; }

        public MessageBrokerHelper(MessageBrokerConfig config)
        {
            _lazyConnection = new Lazy<ConnectionFactory>(() => new ConnectionFactory()
            {
                HostName = config.HostName,
                Port = AmqpTcpEndpoint.UseDefaultPort,
                UserName = config.UserName,
                Password = config.Password
            });
        }
    }
}
