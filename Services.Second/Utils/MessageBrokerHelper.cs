using RabbitMQ.Client;

namespace Services.Second.Utils
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
                Password = config.Password,
                VirtualHost = config.VirtualHost,
            });

            ConfigConnectionFactory();
        }

        private void ConfigConnectionFactory()
        {
            this._lazyConnection.Value.ClientProperties["capabilities"] = new Dictionary<string, object>
            {
                { "publisher_confirms", true },
                { "exchange_exchange_bindings", true },
                { "consumer_cancel_notify", true }
            };
        }
    }
}
