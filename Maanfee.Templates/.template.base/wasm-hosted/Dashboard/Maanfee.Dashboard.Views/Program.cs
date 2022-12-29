using Allegiance.Blazor.Highcharts.Services;
using Maanfee.Dashboard.Views;
using Maanfee.Dashboard.Views.Base.Extensions;
using Maanfee.Dashboard.Views.Booklet;
using Maanfee.Dashboard.Views.Core.Extensions;
using Maanfee.Web.Core;
using Maanfee.Web.JSInterop;
using Maanfee.Web.Printing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;

var builder = WebAssemblyHostBuilder.CreateDefault(args).AddClientServices();

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// ***************************
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
//var HTTP = new HttpClient()
//{
//    BaseAddress = new Uri(builder.HostEnvironment.BaseAddress),
//};
//builder.Services.AddScoped(sp => HTTP);
// **************************

// HighCharts
builder.Services.AddSingleton<IChartService, ChartService>();

// Print
builder.Services.AddScoped<IPrintingService, PrintingService>();

// JsQuery
builder.Services.AddJsQuery();

// Booklet
builder.Services.AddBooklet();

// File Download
builder.Services.AddFileDownload();

var host = builder.Build();

var Config = host.Services.GetRequiredService<LocalConfiguration>();
await Config.InitConfigurationAsync();
await Config.InitWasmCultureAsync(builder.Services);

await host.RunAsync();
