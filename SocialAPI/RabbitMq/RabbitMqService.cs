using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace SocialAPI.RabbitMq
{
    public class RabbitMqService
    {
        private readonly IConfiguration _config;

        public RabbitMqService(IConfiguration config)
        {
            _config = config;
        }

        public void QueueDeclare(string queueName)
        {
            using var channel = RabbitConnection.Connection.CreateModel();

            channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false);
        }

        public void SendMessageToExchange(string message)
        {
            using var channel = RabbitConnection.Connection.CreateModel();

            channel.ExchangeDeclare(_config["RabbitMq:ExchangeName"], ExchangeType.Fanout);

            var messageBytes = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(_config["RabbitMq:ExchangeName"], string.Empty, null, messageBytes);
        }

        public void SendMessageToQueue(string queueName, string message)
        {
            using var channel = RabbitConnection.Connection.CreateModel();

            channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false);

            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(exchange: string.Empty, routingKey: queueName, mandatory: false, basicProperties: null, body: body);

        }

        public async Task<List<string>> GetMessagesAsync(string queueName)
        {
            using var channel = RabbitConnection.Connection.CreateModel();

            var messages = new List<string>();

            channel.ExchangeDeclare(_config["RabbitMq:ExchangeName"], ExchangeType.Fanout);
            channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false);
            channel.QueueBind(queueName, _config["RabbitMq:ExchangeName"], queueName);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                messages.Add(Encoding.UTF8.GetString(ea.Body.ToArray()));
            };

            channel.BasicConsume(queueName, true, consumer);

            while (true)
            {
                await Task.Delay(500);
                if (channel.MessageCount(queueName) == 0 || messages.Count > 10)
                    break;
            }

            return messages;
        }
    }
}
