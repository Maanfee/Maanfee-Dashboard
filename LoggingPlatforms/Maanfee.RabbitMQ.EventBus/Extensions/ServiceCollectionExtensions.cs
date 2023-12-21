using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace Maanfee.RabbitMQ.EventBus
{
    public static class ServiceCollectionExtensions
    {
        public static void AddRabbitMQEventBus(this IServiceCollection services,
            string HostName, string Username, string Password, string BrokerName, string QueueName)
        {
            services.AddSingleton<IEventBus, EventBus>(factory =>
            {
                var ConnectionFactory = new ConnectionFactory
                {
                    HostName = HostName,
                    UserName = Username,
                    Password = Password,
                    DispatchConsumersAsync = true,
                };

                //string url = "wss://test-small-ivory-rat.rmq2.cloudamqp.com/ws/amqp";
                //var amqp = new webm(url, "VHOST", "USERNAME", "YOUR_PASSWORD");

                //var ConnectionFactory = new ConnectionFactory()
                //{

                //};

                //  "amqp://guest:guest@localhost:5672"

                return new EventBus(ConnectionFactory, QueueName);
            });

        }
    }
}
