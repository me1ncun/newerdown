using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NewerDown.Infrastructure.Data;
using NewerDown.Infrastructure.Queuing;
using NewerDown.ServicingFunctions.Services;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

builder.Services
    .AddApplicationInsightsTelemetryWorkerService()
    .ConfigureFunctionsApplicationInsights();

builder.Services.AddSingleton<IQueueSenderFactory, QueueSenderFactory>();
builder.Services.AddTransient<IWebSiteCheckService, WebSiteCheckService>();
builder.Services.AddTransient<IStatisticsService, StatisticsService>();

builder.Services.AddSingleton<ServiceBusClient>(sp => new ServiceBusClient(builder.Configuration["ServiceBusConnection"]));

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration["DatabaseConnection"]);
});

builder.Build().Run();
