using AKSoftware.Blazor.Utilities;
using Blazored.SessionStorage;
using Blazored.Toast;
using ETClient;
using ETClient.Services;
using ETClient.WebSockets;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Plugin.Net.Socket;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddBlazoredToast();
builder.Services.AddHttpContextAccessor();
builder.Services.AddBlazoredSessionStorage();
builder.Services.AddServerSideBlazor().AddHubOptions(config => config.MaximumReceiveMessageSize = 1048576);
builder.Services.AddScoped<BrowserService>();
builder.Services.AddScoped<SesionService>();
builder.Services.AddSignalR();
builder.Services.AddLocalization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");
app.MapHub<MessageHub>("/socket");
app.UseRequestLocalization(new RequestLocalizationOptions()
    .AddSupportedCultures(new[] { "en-US", "es-MX" })
    .AddSupportedUICultures(new[] { "en-US", "es-MX" }));

SocketFactory.Instance.Register(async () =>
{
    await Task.Delay(1);
    return new SocketIO("http://142.11.222.110:9182/");
});

app.Run();
