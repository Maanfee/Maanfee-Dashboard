using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Maanfee.Dashboard.Services.Extensions
{
    public static class AddSwaggerExtension
    {
        public static void AddSwaggerServices(this IServiceCollection Services, IConfiguration Configuration)
        {
            // Register the Swagger generator, defining 1 or more Swagger documents
            Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Version = "v1",
                    Title = "Gateaway API",
                    Description = "System Access Web API",
                    TermsOfService = new Uri("https://opensource.org/licenses/MIT"),
                    Contact = new Microsoft.OpenApi.Models.OpenApiContact
                    {
                        Name = "Docs.Net.Advanced",
                        Email = string.Empty,
                        Url = new Uri("https://opensource.org/licenses/MIT"),
                    },
                    License = new Microsoft.OpenApi.Models.OpenApiLicense
                    {
                        Name = "Use under MFL",
                        Url = new Uri("https://opensource.org/licenses/MIT"),
                    }
                });
            });

        }

    }
}