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

builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("SmtpSettings"));
builder.Services.AddTransient<IEmailService, EmailService>();

 /*builder.Services
     .AddApplicationInsightsTelemetryWorkerService()
     .ConfigureFunctionsApplicationInsights();*/

builder.Build().Run();