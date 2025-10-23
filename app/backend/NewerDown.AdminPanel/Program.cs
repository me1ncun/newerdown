using Azure.Identity;
using Microsoft.AspNetCore.Identity;
using NewerDown.Application;
using NewerDown.Application.Constants;
using NewerDown.Domain.Entities;
using NewerDown.Extensions;
using NewerDown.Infrastructure;
using NewerDown.Infrastructure.Data;
using NewerDown.Shared;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables()
    .AddUserSecrets<Program>(optional: true);

var keyVaultUri = builder.Configuration["AzureKeyVault:VaultUri"];
if (string.IsNullOrEmpty(keyVaultUri))
    throw new ArgumentException("AzureKeyVault:VaultUri is not configured.");

builder.Configuration.AddAzureKeyVault(new Uri(keyVaultUri), new DefaultAzureCredential());

var configuration = builder.Configuration;

builder.Services.AddRazorPages();
        
builder.Services.AddIdentity<User, IdentityRole<Guid>>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddCORS();

builder.Services.AddSignalR().AddAzureSignalR(configuration["SignalRConnection"]);;
        
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(SessionConstants.DefaultIdleTimeoutInMinutes);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
        
builder.Services.AddHttpContextAccessor();

builder.Services.AddSharedServices(configuration);
builder.Services.AddApplicationServices(configuration);
builder.Services.AddInfrastructureServices(configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();

app.UseAuthorization();

app.MapRazorPages();

app.Run();