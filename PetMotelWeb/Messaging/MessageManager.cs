using System;
using System.Text;
using Common;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace PetMotelWeb.Messaging
{

    public class MessageManager : IDisposable
    {
        private readonly IModel _channel;
        private readonly ILogger _logger;
        
        public MessageManager(ILogger logger)
        {
        }

        public void SendExampleEmail(IExampleEmail email)
        {
            
        }

        public void Dispose()
        {
            _channel?.Dispose();
        }
    }

    // public class MessageManager : IDisposable
    // {
    //     private readonly IModel _channel;
    //     private readonly ILogger _logger;
    //
    //     public MessageManager(ILogger logger)
    //     {
    //         try
    //         {
    //             logger.LogInformation("Setup rabbit");
    //             ConnectionFactory factory = 
    //                 new ConnectionFactory {Uri = new Uri(RabbitMqConstants.RabbitMqUri)};
    //             logger.LogInformation("Creating con");
    //             IConnection connection = factory.CreateConnection();
    //             _channel = connection.CreateModel();
    //         }
    //         catch (Exception e)
    //         {
    //             logger.LogError($"Error creating with {e.Message}\n{e.StackTrace}", e);
    //         }
    //         
    //         _logger = logger;
    //     }
    //
    //     public void SendExampleEmail(IExampleEmail email)
    //     {
    //         try
    //         {
    //             _logger.LogInformation("Setting up channels");
    //             _channel.ExchangeDeclare(exchange: RabbitMqConstants.TestExchange, type: ExchangeType.Direct);
    //             _channel.QueueDeclare(queue: RabbitMqConstants.TestQueue, durable: false, exclusive: false,
    //                 autoDelete: false, arguments: null);
    //             _channel.QueueBind(queue: RabbitMqConstants.TestQueue, exchange: RabbitMqConstants.TestExchange,
    //                 routingKey: "");
    //
    //             _logger.LogInformation("Processing message");
    //             string serializeObject = JsonConvert.SerializeObject(email);
    //             IBasicProperties basicProperties = _channel.CreateBasicProperties();
    //             basicProperties.ContentType = RabbitMqConstants.JsonMimeType;
    //
    //             _logger.LogInformation("Sending message");
    //             _channel.BasicPublish(exchange: RabbitMqConstants.TestExchange,
    //                 routingKey: "", basicProperties: basicProperties, body: Encoding.UTF8.GetBytes(serializeObject));
    //             
    //             _logger.LogInformation("Done");
    //         }
    //         catch (Exception e)
    //         {
    //             _logger.LogError("Error sending rabbit message", e);
    //         }
    //     }
    //
    //     public void Dispose()
    //     {
    //         if (!_channel.IsClosed)
    //         {
    //             _channel.Close();
    //         }
    //     }
    // }
}