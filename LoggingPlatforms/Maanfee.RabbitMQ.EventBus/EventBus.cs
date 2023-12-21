using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace Maanfee.RabbitMQ.EventBus
{
    public class EventBus : IEventBus, IDisposable
    {
        public EventBus(IConnectionFactory connectionFactory, string queueName)
        {
            ConnectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
            QueueName = queueName ?? throw new ArgumentNullException(nameof(queueName));
        }

        private readonly IConnectionFactory ConnectionFactory;
        private readonly string QueueName;

        public void Publish<T>(T message)
        {
            var connection = ConnectionFactory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(QueueName, exclusive: false);

            var json = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(json);

            channel.BasicPublish(exchange: "", routingKey: QueueName, body: body);
        }

        //public void Subscribe()
        //{
        //    var connection = ConnectionFactory.CreateConnection();
        //    using var channel = connection.CreateModel();

        //    channel.QueueDeclare(QueueName, exclusive: false);

        //    var consumer = new EventingBasicConsumer(channel);

        //    consumer.Received += (model, eventArgs) =>
        //    {
        //        var body = eventArgs.Body.ToArray();
        //        var message = Encoding.UTF8.GetString(body);

        //        //ThresholdReachedEventArgs args = new ThresholdReachedEventArgs();
        //        //args.Threshold = 1000;
        //        //args.TimeReached = DateTime.Now;
        //        //OnThresholdReached(args);

        //        Console.WriteLine($"Message received: {message}");
        //    };

        //    //Task.Run(() =>
        //    //{
        //        channel.BasicConsume(queue: QueueName, autoAck: true, consumer: consumer);
        //    //});
        //}

        protected virtual void OnThresholdReached(ThresholdReachedEventArgs e)
        {
            EventHandler<ThresholdReachedEventArgs> handler = ThresholdReached;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public event EventHandler<ThresholdReachedEventArgs> ThresholdReached;

        public void Dispose()
        {
            //_connection.Dispose();
        }

    }

    public class ThresholdReachedEventArgs : EventArgs
    {
        public int Threshold { get; set; }
        public DateTime TimeReached { get; set; }
    }
}