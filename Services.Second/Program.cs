using Services.Second;
using Services.Second.Utils;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

builder.Services.AddSingleton<MessageBrokerConfig>();
builder.Services.AddSingleton<MessageBrokerHelper>();
builder.Services.AddSingleton<CronJobConfig>();

var host = builder.Build();
host.Run();
