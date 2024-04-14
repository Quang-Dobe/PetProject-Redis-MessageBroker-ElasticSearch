using Services.Data.Core;
using Services.Second.Services;
using Services.Second.Services.Abstraction;
using Services.Second.Utils;

namespace Services.Second
{
    public class ElasticWorker : CronJobBackgroundService
    {
        private readonly ElasticClient _elasticClient;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<ElasticWorker> _logger;

        public ElasticWorker(CronJobConfig config,
            ElasticClient elasticClient,
            IServiceProvider serviceProvider,
            ILogger<ElasticWorker> logger) : base(config)
        {
            _elasticClient = elasticClient;
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task DoWork(CancellationToken stoppingToken)
        {
            if (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    _logger.LogInformation("Starting syncing data to Elastic DB...");

                    if (!_elasticClient.ExistsIndex)
                    {
                        await _elasticClient.CreateIndexAsync();
                    }

                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var dbContext = scope.ServiceProvider.GetService<ServiceDbContext>();
                        var elasticServices = scope.ServiceProvider.GetService<IElasticServices>();

                        var usersNotSynced = dbContext.Set<User>().Where(x => x.IsSync == false);
                        var deviceNotSynced = dbContext.Set<Device>().Where(x => x.IsSync == false);

                        var responseForSyncingUser = await elasticServices.BulkIndexAsync(_elasticClient, usersNotSynced);
                        var responseForSyncingDevice = await elasticServices.BulkIndexAsync(_elasticClient, deviceNotSynced);

                        foreach (var user in usersNotSynced)
                        {
                            user.IsSync = true;
                        }

                        foreach (var device in deviceNotSynced)
                        {
                            device.IsSync = true;
                        }

                        await dbContext.SaveChangesAsync();
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
