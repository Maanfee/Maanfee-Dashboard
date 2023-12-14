using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace Maanfee.RabbitMQ.EventBus
{
    public static class ServiceCollectionExtensions
	{
		public static void AddRabbitMQEventBus(this IServiceCollection services,
			string HostName, string Username, string Password, string brokerName, string QueueName)
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

				return new EventBus(ConnectionFactory, QueueName);
			});

		}
	}
}
