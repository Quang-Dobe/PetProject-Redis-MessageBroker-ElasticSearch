using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Services.Second.Utils;
using System.Text;

namespace Services.Second
{
    public class Worker : CronJobBackgroundService
    {
        private readonly ConnectionFactory _connectionFactory;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<Worker> _logger;

        public Worker(IServiceProvider serviceProvider, 
            MessageBrokerHelper messageBrokerHelper, 
            ILogger<Worker> logger, 
            CronJobConfig config) : base(config)
        {
            _connectionFactory = messageBrokerHelper.Connection;
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task DoWork(CancellationToken stoppingToken)
        {
            if (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        using var connection = _connectionFactory.CreateConnection();

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

                        using (var channel = connection.CreateModel())
                        {
                            var queueName = "product";
                            channel.QueueDeclare(queueName, exclusive: false);
                            // Size of each prefetch: Unlimited, number of prefetch (number of message for receiving at the same time): 2
                            // This applies for any consumer. (If false, just applies separately to each new consumer - define BasicQos multiple time)
                            channel.BasicQos(prefetchSize: 0, prefetchCount: 2, global: true);

                            var consumer = new EventingBasicConsumer(channel);
                            consumer.Received += (sender, args) =>
                            {
                                // Ack manually only if consumer received message sucessfully (autoAck to false)
                                channel.BasicAck(args.DeliveryTag, false);

                                var body = args.Body.ToArray();
                                var message = Encoding.UTF8.GetString(body);
                                var model = JsonConvert.DeserializeObject<string>(message);

                                _logger.LogInformation(model);
                            };
                            consumer.Shutdown += (sender, args) =>
                            {
                                // Ack manually only if consumer received message unsucessfully (autoAck to false)
                                channel.BasicNack(args.MethodId, true, true);

                                _logger.LogWarning($"-------------------------------------");
                                _logger.LogWarning($"Queue name {queueName} is terminated");
                                _logger.LogWarning($"Reply Code: {args.ReplyCode}");
                                _logger.LogWarning($"Reply Text: {args.ReplyText}");
                            };

                            var result = channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);
                            _logger.LogInformation($"Process ID: {result.Split("-")[1]}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                }
            }
        }
    }
}
