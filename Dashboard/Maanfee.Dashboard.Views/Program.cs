using Maanfee.Dashboard.Views;
using Maanfee.Dashboard.Views.Core;
using Maanfee.Dashboard.Views.Core.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;
using System.Threading.Tasks;

var builder = WebAssemblyHostBuilder.CreateDefault(args).AddDashboardClientServices();

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// GatewayApi
builder.Services.AddScoped<GatewayApi>();

var host = builder.Build();

var Config = host.Services.GetRequiredService<LocalConfigurationService>();
await Config.InitConfigurationAsync(LanguageManager.SupportedCountry.US);
await InitCultureAsync(builder.Services);

await host.RunAsync();

async Task InitCultureAsync(IServiceCollection Services)
{
    Services.AddLocalization(options =>
    {
        options.ResourcesPath = "Resources";
    });

    var Culture = new CultureInfo(SharedLayoutSettings.CultureCode);

    CultureInfo.DefaultThreadCurrentCulture = Culture;
    CultureInfo.DefaultThreadCurrentUICulture = Culture;
    CultureInfo.CurrentCulture = Culture;
    CultureInfo.CurrentUICulture = Culture;
}
