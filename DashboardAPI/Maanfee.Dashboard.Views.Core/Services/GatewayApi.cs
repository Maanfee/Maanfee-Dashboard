using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace Maanfee.Dashboard.Views.Core.Services
{
    public class GatewayApi
    {
        public GatewayApi(IWebAssemblyHostEnvironment _HostEnvironment)
        {
            HostEnvironment = _HostEnvironment;
        }

        private IWebAssemblyHostEnvironment HostEnvironment;

        public string ToUri
        {
            get
            {
                if (HostEnvironment.IsDevelopment())
                    return "http://localhost:4030/gateway";
                else
                    return "http://172.17.17.22:4030/gateway";
            }
        }

    }
}
