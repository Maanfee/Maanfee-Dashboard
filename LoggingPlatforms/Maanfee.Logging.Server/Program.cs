using Maanfee.Logging.Console;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.SignalR.Client;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//var provider = builder.Services.BuildServiceProvider();
//var LoggingHubConnection = provider.GetRequiredService<HubConnection>();

builder.Services.AddSignalR();
builder.Services.AddResponseCompression(opts =>
{
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
        new[] { "application/octet-stream" });
});

// RealTime Logging
var sp = builder.Services.AddLoggingConsole(new Uri("http://localhost:22001"), new Uri("http://172.17.17.22"));
var LoggingHubConnection = sp.BuildServiceProvider().GetRequiredService<HubConnection>();

var app = builder.Build();

// Configure the HTTP request pipeline.

var LogMessage = new LogInfo();

app.MapGet("/weatherforecast", () =>
{
    LoggingHubConnection.On<LogInfo>("ReceiveMessage", (Log) =>
    {
        LogMessage.Platform = Log.Platform;
        LogMessage.Message = Log.Message;
        LogMessage.LogDate = Log.LogDate;
        LogMessage.Level = Log.Level;
    });
    return LogMessage.Message ?? "TEXT";
});

app.Run();
