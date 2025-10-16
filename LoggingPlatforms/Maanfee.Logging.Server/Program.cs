using Maanfee.Logging.Console;
using Maanfee.Logging.Domain.DAL;
using Maanfee.Logging.Server.Data;
using Maanfee.Logging.Server.Services;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Fixed Self referencing loop detected with type and CaseInsensitive
builder.Services.AddControllers()
    .AddNewtonsoftJson(options => { options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore; })
    .AddJsonOptions(options => { options.JsonSerializerOptions.PropertyNameCaseInsensitive = false; });

// Add Databases services.
#region - Configuring Database -

builder.Services.AddDbContext<_BaseContext_SQLServer>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SQLServerConnection_DebugMode"));
});

#endregion

// Fixed Self referencing loop detected with type and CaseInsensitive
builder.Services.AddControllers()
    .AddNewtonsoftJson(options => { options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore; })
    .AddJsonOptions(options => { options.JsonSerializerOptions.PropertyNameCaseInsensitive = false; });

builder.Services.AddResponseCompression(opts =>
{
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
        new[] { "application/octet-stream" });
});

// RealTime Logging
builder.Services.AddLoggingConsole(new Uri("http://localhost:22001"), new Uri("http://172.17.17.22"));
builder.Services.AddSingleton<HttpClient>();
builder.Services.AddHostedService<LogStoreService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    var ContextOptions = new DbContextOptionsBuilder<_BaseContext_SQLServer>()
        .UseSqlServer(builder.Configuration.GetConnectionString("SQLServerConnection_DebugMode")).Options;
    var SQLServerContext = new _BaseContext_SQLServer(ContextOptions);

    DbInitializer<_BaseContext_SQLServer>.DatabaseCreating(SQLServerContext);
    SQLServerDataInitializer.Initialize(SQLServerContext);
}

app.UseAuthorization();

app.MapControllers();

app.Run();
