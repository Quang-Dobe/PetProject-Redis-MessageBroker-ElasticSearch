using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Services.Second.Utils;
using System.Diagnostics.Metrics;
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
                        var action = new Action<string>(message => DoSomethingHere(message));

                        using var connection = _connectionFactory.CreateConnection();
                        using (var channel = connection.CreateModel())
                        {
                            var queueName = "product";
                            channel.QueueDeclare(queueName, exclusive: false);

                            var consumer = new EventingBasicConsumer(channel);
                            consumer.Received += (sender, args) =>
                            {
                                var body = args.Body.ToArray();
                                var message = Encoding.UTF8.GetString(body);
                                var model = JsonConvert.DeserializeObject<string>(message);

                                _logger.LogInformation(model);
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

        private void DoSomethingHere(string message)
        {
            _logger.LogInformation(message);
        }
    }
}
