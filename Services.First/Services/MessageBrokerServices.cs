using Newtonsoft.Json;
using RabbitMQ.Client;
using Services.First.Services.Abstraction;
using Services.First.Utils;
using System;
using System.Text;

namespace Services.First.Services
{
    public class MessageBrokerServices : IMessageBrokerServices
    {
        private readonly ConnectionFactory _connectionFactory;
        private readonly ILogger<MessageBrokerServices> _logger;

        public MessageBrokerServices(MessageBrokerHelper messageBrokerHelper, ILogger<MessageBrokerServices> logger)
        {
            _connectionFactory = messageBrokerHelper.Connection;
            _logger = logger;

        }

        public void SendMessage<T>(T message)
        {
            var connection = _connectionFactory.CreateConnection();

            // Log information about this connection...
            _logger.LogInformation($"Connection {connection.ClientProvidedName} has been starting...");

            // Define when a connection get block/unblock
            connection.ConnectionBlocked += (sender, e) =>
                _logger.LogTrace($"Connection of {connection.ClientProvidedName} is blocked! \nReason: {e.Reason}");
            connection.ConnectionUnblocked += (sender, e) =>
                _logger.LogTrace($"Connection of {connection.ClientProvidedName} is unblocked!");

            // Define when a connection get an exception
            connection.CallbackException += (sender, e) =>
                _logger.LogCritical($"There are something failed here. Please have a check! \nReason: {e.Exception.Message}");

            using var channel = connection.CreateModel();
            var queueName = "product";
            channel.QueueDeclare(queueName, exclusive: false);
            channel.BasicReturn += (sender, e) => channel.BasicAck(e.ReplyCode, false);
            channel.ModelShutdown += (sender, e) => channel.BasicNack(e.ReplyCode, false, true);
            
            var json = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(json);

            // Define UserId for further debugging

            // Just use Default Exchange (Exchange with empty name)
            // Type of Exchange: Direct
            // Queue-Binding: Implicit Binding - Routing key equals Queue's name
            channel.BasicPublish(exchange: "", routingKey: queueName, body: body);
        }

        public void ReceivedMessage<T>(Action<T> action)
        {
            throw new NotImplementedException();
        }
    }
}
