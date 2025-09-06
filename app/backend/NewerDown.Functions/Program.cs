using Azure.Identity;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NewerDown.Application.Services;
using NewerDown.Domain.Interfaces;
using NewerDown.Functions.Models;
using NewerDown.Functions.Services;
using NewerDown.Infrastructure.Data;
using NewerDown.Infrastructure.Queuing;
using IMonitorService = NewerDown.Functions.Services.IMonitorService;
using MonitorService = NewerDown.Functions.Services.MonitorService;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

builder.Services
    .AddApplicationInsightsTelemetryWorkerService()
    .ConfigureFunctionsApplicationInsights();

var kvUri = Environment.GetEnvironmentVariable("AzureKeyVault");

builder.Configuration.AddAzureKeyVault(new Uri(kvUri ?? "https://kv-newerdown.vault.azure.net/"), new DefaultAzureCredential());

builder.Services.AddOptions<EmailSettings>()
    .Configure<IConfiguration>((settings, config) =>
    {
        settings.Host = config["SmtpHost"];
        settings.Port = int.Parse(config["SmtpPort"]);
        settings.UserName = config["SmtpUsername"];
        settings.Password = config["SmtpPassword"];
        settings.FromEmail = config["SmtpFromEmail"];
        settings.FromName = config["SmtpFromName"];
        settings.EnableSsl = true;
    });

builder.Services.AddTransient<IEmailService, EmailService>();
builder.Services.AddTransient<INotificationService, NotificationService>();
builder.Services.AddSingleton<IQueueSenderFactory, QueueSenderFactory>();
builder.Services.AddTransient<IMonitorService, MonitorService>();
builder.Services.AddTransient<IStatisticsService, StatisticsService>();

builder.Services.AddSingleton<ServiceBusClient>(sp => new ServiceBusClient(builder.Configuration["ServiceBusConnection"]));

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration["DatabaseConnection"]);
});

builder.Build().Run();