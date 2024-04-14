using Microsoft.EntityFrameworkCore;
using Services.Second;
using Services.Second.Services;
using Services.Second.Services.Abstraction;
using Services.Second.Utils;

var builder = Host.CreateApplicationBuilder(args);
//builder.Services.AddHostedService<Worker>();
builder.Services.AddHostedService<ElasticWorker>();

// Add default DBConnection
builder.Services.AddDbContext<ServiceDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("ServiceDb")));

builder.Services.AddScoped<IElasticServices, ElasticServices>();
builder.Services.AddSingleton<MessageBrokerConfig>();
builder.Services.AddSingleton<MessageBrokerHelper>();
builder.Services.AddSingleton<CronJobConfig>();
builder.Services.AddSingleton<ElasticConfig>();
builder.Services.AddSingleton<ElasticHelper>();
builder.Services.AddSingleton<ElasticClient>();

var host = builder.Build();
host.Run();
