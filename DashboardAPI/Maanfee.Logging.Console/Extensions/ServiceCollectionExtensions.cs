using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;

namespace Maanfee.Logging.Console
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddLoggingConsole(this IServiceCollection services)
        {
            services.AddSingleton<HubConnection>(sp =>
            {
                var NavigationManager = sp.GetRequiredService<NavigationManager>();

                return new HubConnectionBuilder()
                     .WithUrl(NavigationManager.ToAbsoluteUri("/LoggingHub"))
                     .WithAutomaticReconnect()
                     .Build();
            });

            services.AddSingleton<LoggingInitializer>();

            return services;
        }
    }
}
