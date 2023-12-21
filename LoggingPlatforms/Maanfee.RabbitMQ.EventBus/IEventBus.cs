namespace Maanfee.RabbitMQ.EventBus
{
    public interface IEventBus
	{
		void Publish<T>(T message);

        //void Subscribe();
	}
}
