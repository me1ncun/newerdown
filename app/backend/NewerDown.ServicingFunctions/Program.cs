using Azure.Identity;
using Azure.Messaging.ServiceBus;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NewerDown.Infrastructure.Data;
using NewerDown.Infrastructure.Queuing;
using NewerDown.ServicingFunctions.Options;
using NewerDown.ServicingFunctions.Services;

var builder = FunctionsApplication.CreateBuilder(args);

var keyVaultUrl = builder.Configuration["KeyVaultUrl"];
if (!string.IsNullOrEmpty(keyVaultUrl))
{
    builder.Configuration.AddAzureKeyVault(
        new Uri(keyVaultUrl),
        new DefaultAzureCredential());
}

builder.ConfigureFunctionsWebApplication();

builder.Services
    .AddApplicationInsightsTelemetryWorkerService()
    .ConfigureFunctionsApplicationInsights();

builder.Services.AddSingleton<IQueueSenderFactory, QueueSenderFactory>();
builder.Services.AddTransient<IWebSiteCheckService, WebSiteCheckService>();
builder.Services.AddTransient<IStatisticsService, StatisticsService>();
builder.Services.AddTransient<IEmailSender, EmailService>();

builder.Services.AddSingleton<ServiceBusClient>(sp => new ServiceBusClient(builder.Configuration["ServiceBusConnection"]));

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration["DataConnection"]);
});

builder.Services.AddOptions<SmtpOptions>()
    .Bind(builder.Configuration.GetSection(SmtpOptions.Smtp))
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Build().Run();
