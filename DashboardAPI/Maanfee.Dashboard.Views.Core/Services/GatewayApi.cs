using Maanfee.Dashboard.Domain.ViewModels;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using System.Text.Json;

namespace Maanfee.Dashboard.Views.Core.Services
{
    public class GatewayApi
    {
        public GatewayApi(IWebAssemblyHostEnvironment? _HostEnvironment, HttpClient? httpClient)
        {
            HostEnvironment = _HostEnvironment;
            Http = httpClient;
        }

        private readonly IWebAssemblyHostEnvironment? HostEnvironment;

        private readonly HttpClient? Http;

        public async Task InitializeAsync()
        {
            //GatewaySettings = await Http!.GetFromJsonAsync<GatewaySettings>("Gateway.json") ?? new GatewaySettings();
            var json = await Http!.GetStringAsync("Gateway.json");
            var doc = JsonDocument.Parse(json);
            var settings = doc.RootElement.GetProperty("GatewaySettings");
            GatewaySettings = JsonSerializer.Deserialize<GatewaySettings>(settings);

            if (GatewaySettings == null)
            {
                GatewaySettings = new GatewaySettings
                {
                    DebugIP = "localhost",
                    DebugPort = "4030",
                    ReleaseIP = "192.168.1.100",
                    ReleasePort = "4030"
                };
                Console.WriteLine("Warning: Gateway.json not found. Using default settings.");
            }
        }

        private GatewaySettings? GatewaySettings { get; set; }

        public string ToUri
        {
            get
            {
                if (HostEnvironment!.IsDevelopment())
                    return $"http://{GatewaySettings!.DebugIP}:{GatewaySettings!.DebugPort}/gateway";
                else
                    return $"http://{GatewaySettings!.ReleaseIP}:{GatewaySettings!.ReleasePort}/gateway";
            }
        }
    }
}
