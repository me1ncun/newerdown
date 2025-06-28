using Azure.Identity;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NewerDown.Functions.Models;
using NewerDown.Functions.Services;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

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

 /*builder.Services
     .AddApplicationInsightsTelemetryWorkerService()
     .ConfigureFunctionsApplicationInsights();*/

builder.Build().Run();