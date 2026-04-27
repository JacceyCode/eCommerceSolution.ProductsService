using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace BusinessLogicLayer.RabbitMQ;

public class RabbitMQPublisher : IRabbitMQPublisher, IAsyncDisposable
{
    private readonly IConfiguration _configuration;
    private readonly ConnectionFactory _connectionFactory;
    private IConnection? _connection;
    private IChannel? _channel;
    private readonly SemaphoreSlim _connectionLock = new(1, 1);

    public RabbitMQPublisher(IConfiguration configuration)
    {
        _configuration = configuration;

        string hostname = _configuration["RabbitMQ_HostName"]!;
        string userName = _configuration["RabbitMQ_UserName"]!;
        string password = _configuration["RabbitMQ_Password"]!;
        string port = _configuration["RabbitMQ_Port"]!;

        _connectionFactory = new ConnectionFactory()
        {
            HostName = hostname,
            UserName = userName,
            Password = password,
            Port = int.Parse(port ?? "5672")
        };


    }

    private async Task ConnectAsync()
    {
        if (_connection != null && _connection.IsOpen) return;

        await _connectionLock.WaitAsync();
        try
        {
            if (_connection == null || !_connection.IsOpen)
            {
                // Establish a connection to RabbitMQ and create a channel
                _connection = await _connectionFactory.CreateConnectionAsync();
                _channel = await _connection.CreateChannelAsync();
            }
        }
        finally
        {
            _connectionLock.Release();
        }
    }
    public async Task Publish<T>(string routingKey, T message)
    {
        await ConnectAsync();

        string messageJson = JsonSerializer.Serialize(message);
        byte[] messageBytes = Encoding.UTF8.GetBytes(messageJson);

        // Create exchange
        string exchangeName = _configuration["RabbitMQ_Products_Exchange_Name"]!;
        await _channel!.ExchangeDeclareAsync(
            exchange: exchangeName, 
            type: ExchangeType.Direct,
            durable: true,
            autoDelete: false,
            arguments: null
            );

        // Publish the message to the exchange with the specified routing key
        await _channel.BasicPublishAsync(
            exchange: exchangeName, 
            routingKey: routingKey,
            mandatory: false, 
            basicProperties: new BasicProperties(),
            //{
                //DeliveryMode = DeliveryModes.Persistent,
                //ContentType = "application/json"
            //},
            body: messageBytes
            );
    }

    public async ValueTask DisposeAsync()
    {
        if (_channel != null) await _channel.DisposeAsync();
        if (_connection != null) await _connection.DisposeAsync();
        _connectionLock.Dispose();
    }
}
