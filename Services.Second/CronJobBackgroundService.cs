using Quartz;
using Services.Second.Utils;

namespace Services.Second
{
    public abstract class CronJobBackgroundService : BackgroundService
    {
        private readonly string _scheduleTime;

        public CronJobBackgroundService(CronJobConfig config)
        {
            _scheduleTime = config.ScheduleTime;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var cron = new CronExpression(_scheduleTime);
            var next = cron.GetNextValidTimeAfter(DateTimeOffset.Now);

            while (!stoppingToken.IsCancellationRequested)
            {
                if (DateTimeOffset.Now > next)
                {
                    await DoWork(stoppingToken);

                    next = cron.GetNextValidTimeAfter(DateTimeOffset.Now);
                }

                await Task.Delay(1000, stoppingToken);
            }
        }

        protected abstract Task DoWork(CancellationToken stoppingToken);
    }
}
