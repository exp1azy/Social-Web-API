using RabbitMQ.Client;

namespace SocialAPI.RabbitMq
{
    public class RabbitConnection
    {
        private static IConnection? _connection = null;

        private static readonly object _lockObject = new();

        public static IConnection Connection
        {
            get
            {
                lock (_lockObject)
                {
                    if (_connection == null || !_connection.IsOpen)
                    {
                        var connectionFactory = new ConnectionFactory() { HostName = "localhost" };
                        _connection = connectionFactory.CreateConnection();
                    }

                    return _connection;
                }
            }
        }
    }
}
