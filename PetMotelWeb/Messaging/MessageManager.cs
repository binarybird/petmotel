using System;
using System.Text;
using System.Threading;
using Common;
using MassTransit;
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
            _logger = logger;
        }
    
        public async void SendExampleEmail(IExampleEmail email)
        {
            // _logger.LogInformation("Init bus control");
            // var busControl = Bus.Factory.CreateUsingRabbitMq(config =>
            // {
            //     config.Host(RabbitMqConstants.RabbitMqUri);
            //     config.ReceiveEndpoint(RabbitMqConstants.IdentityLoginLogoutQueue, x =>
            //     {
            //         x.Bind(RabbitMqConstants.IdentityExchange);
            //     });
            // });
            //
            // _logger.LogInformation("Start Async");
            // var source = new CancellationTokenSource(TimeSpan.FromSeconds(10));
            // await busControl.StartAsync(source.Token);
            // try
            // {
            //     _logger.LogInformation("Publishing");
            //     await busControl.Publish<ExampleEmail>(new
            //     {
            //         email = email.Email
            //     });
            // }
            // catch (Exception e)
            // {
            //     _logger.LogError($"{e.Message}\n{e.StackTrace}");
            // }
            // finally
            // {
            //     await busControl.StopAsync();
            // }
            // _logger.LogInformation("Done");
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