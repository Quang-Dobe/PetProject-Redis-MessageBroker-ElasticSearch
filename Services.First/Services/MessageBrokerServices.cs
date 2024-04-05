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

        public MessageBrokerServices(MessageBrokerHelper messageBrokerHelper)
        {
            _connectionFactory = messageBrokerHelper.Connection;
        }

        public void SendMessage<T>(T message)
        {
            var connection = _connectionFactory.CreateConnection();
            var randomQueue = "product";

            using var channel = connection.CreateModel();
            channel.QueueDeclare(randomQueue, exclusive: false);
            
            var json = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(json);

            channel.BasicPublish(exchange: "", routingKey: randomQueue, body: body);
        }

        public void ReceivedMessage<T>(Action<T> action)
        {
            throw new NotImplementedException();
        }

        private string RandomString(int length)
        {
            var random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
