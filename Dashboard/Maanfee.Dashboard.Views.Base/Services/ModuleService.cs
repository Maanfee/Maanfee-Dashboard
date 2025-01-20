﻿using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using System.Net.Http;

namespace Maanfee.Dashboard.Views.Base.Services
{
    public static class ModuleService
    {
        public static ModuleViewModel LogServer { get; set; } = new();
    }

    public class ModuleDefaultValue
    {
        public static string LogServer = "LogServer";
    }

    public class ModuleViewModel
    {
        public string Name { get; set; }

        public bool IsActive { get; set; }

        public bool AutoRoute { get; set; }

        public string LocalUri { get; set; }

        public string GlobalUri { get; set; }

        public string ToExactUri(HttpClient Http)
        {
            if (
                (Http.BaseAddress.AbsoluteUri.Contains("192") ||
                Http.BaseAddress.AbsoluteUri.Contains("127") ||
                Http.BaseAddress.AbsoluteUri.Contains("localhost")) && this.AutoRoute
                )
            {
                return this.LocalUri;
            }
            else
            {
                return this.GlobalUri;
            }
        }

        public bool CanNavigation { get; set; }

        public bool HasJwt { get; set; }
    }

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
                    return "http://1.1.1.1:4030/gateway";
            }
        }

    }
}
