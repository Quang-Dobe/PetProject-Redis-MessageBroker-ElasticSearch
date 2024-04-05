namespace Services.Second.Utils
{
    public class CronJobConfig
    {
        public CronJobConfig(IConfiguration configuration)
        {
            ScheduleTime = configuration["CronJobConfig:ScheduleTime"];
        }

        public string ScheduleTime {  get; set; }
    }
}
