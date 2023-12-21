﻿using Maanfee.RabbitMQ.EventBus;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

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

            RabbitMQEventBusInitializer(services);

            return services;
        }

        public static IServiceCollection AddLoggingConsole(this IServiceCollection services, Uri LocalAddress, Uri PhysicalAddress)
        {
            if(LocalAddress is null || PhysicalAddress == null)
            {
                throw new ArgumentNullException("Address Error ...");
            }

            services.AddSingleton<HubConnection>(sp =>
            {
                var HostEnvironment = sp.GetRequiredService<IHostingEnvironment>();

                if (HostEnvironment.IsDevelopment())
                {
                    return new HubConnectionBuilder()
                           .WithUrl($"{LocalAddress}LoggingHub")
                           .WithAutomaticReconnect()
                           .Build();
                }

                return new HubConnectionBuilder()
                           .WithUrl($"{PhysicalAddress}LoggingHub")
                           .WithAutomaticReconnect()
                           .Build();

                //return new HubConnectionBuilder()
                //         .WithUrl("http://localhost:22001/LoggingHub")
                //         .WithAutomaticReconnect()
                //         .Build();
            });

            services.AddSingleton<LoggingInitializer>();

            RabbitMQEventBusInitializer(services);

            return services;
        }

        #region - Event Bus -

        private static void RabbitMQEventBusInitializer(IServiceCollection services)
        {
            // Add services to the container.
            services.AddRabbitMQEventBus
                        (
                            HostName: "localhost",
                            Username: "guest",
                            Password: "guest",
                            BrokerName: "netCoreEventBusBroker",
                            QueueName: "Queue"
                        );
        }

        #endregion

    }
}