using Maanfee.Dashboard.Services;
using Maanfee.Logging.Console;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Net.Http;
// Server

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddSignalR();

// RealTime Logging
builder.Services.AddLoggingConsole(new Uri("http://localhost:22001"), new Uri("http://172.17.17.22"));

builder.Services.AddLocalization(options =>
            {
                options.ResourcesPath = "Resources";
            });

builder.Services.AddDashboardCompression();
builder.Services.AddDashboardApplicationCookieConfigurations();
builder.Services.AddDashboardDatabaseConfigurations(builder.Configuration);
builder.Services.AddDashboardControllers();
builder.Services.AddSingleton<HttpClient>();
builder.Services.AddScoped<CommonService>();

// ********************************************
builder.Services.AddSwaggerServices(builder.Configuration);
// ********************************************

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
}

app.UseBlazorFrameworkFiles();  //
app.UseStaticFiles();//
try
{
    app.UseStaticFiles(new StaticFileOptions
    {
        FileProvider = new PhysicalFileProvider(Path.Combine(app.Environment.ContentRootPath, "D:\\_UploadedDocuments")),
        RequestPath = "/StaticFiles"
    });
}
catch
{

}

// *************************** Swagger ***************************
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", typeof(Program).Assembly.GetName().Name);
    options.RoutePrefix = "swagger";
    options.DisplayRequestDuration();
});
// ***************************************************************

app.UseRouting();  //
app.UseDashboardCompression();
app.UseAntiforgery();

app.UseRequestLocalization(options =>
{
    var supportedCultures = new[] { "en-US", "fa-IR" };
    options.SetDefaultCulture(supportedCultures[1])
        .AddSupportedCultures(supportedCultures)
        .AddSupportedUICultures(supportedCultures);
});

//app.AddConfigure(env, SQLServerContext, SQLiteContext);
app.UseDashboardDatabaseConfigurations();
app.UseDashboardAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapRazorPages();
    endpoints.MapControllers();
    endpoints.MapFallbackToFile("index.html");
    endpoints.MapHub<LoggingHub>($"/{HubName.Name}");
});

app.Run();


