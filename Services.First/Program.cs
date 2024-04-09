using Microsoft.EntityFrameworkCore;
using Services.First;
using Services.First.Repositories;
using Services.First.Repositories.Abstraction;
using Services.First.Services;
using Services.First.Services.Abstraction;
using Services.First.Utils;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ServiceDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("ServiceDb")));

builder.Services.AddScoped<IUserServices, UserServices>();
builder.Services.AddScoped<IDeviceServices, DeviceServices>();
builder.Services.AddScoped<IUserRepositories, UserRepositories>();
builder.Services.AddScoped<IDeviceRepositories, DeviceRepositories>();
builder.Services.AddScoped<ICacheServices, CacheServices>();
builder.Services.AddScoped<IMessageBrokerServices, MessageBrokerServices>();
builder.Services.AddSingleton<MessageBrokerHelper>();
builder.Services.AddSingleton<MessageBrokerConfig>();
builder.Services.AddSingleton<RedisHelper>();
builder.Services.AddSingleton<RedisConfig>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
