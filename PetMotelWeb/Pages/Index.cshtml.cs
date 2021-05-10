using System;
using System.Runtime.Serialization;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using Common;
using MassTransit;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using PetMotelWeb.Messaging;
using RabbitMQ.Client;

namespace PetMotelWeb.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IBusControl _bus;

        public IndexModel(ILogger<IndexModel> logger, IBusControl bus)
        {
            _bus = bus;
            _logger = logger;
        }

        public void OnGet()
        {

        }
        
        // public void OnPost()
        // {
        //     var emailAddress = Request.Form["search"];
        //     _logger.LogInformation($"Form: {emailAddress}");
        //
        //     _logger.LogInformation("Start");
        //
        //     string rmqUser = null;
        //     string rmqPass = null;
        //     string key = null;
        //     string cert = null;
        //     try
        //     {
        //         string rmqUserPath = Environment.GetEnvironmentVariable("RMQU");
        //         string rmqPassPath = Environment.GetEnvironmentVariable("RMQP");
        //
        //         rmqUser = System.IO.File.ReadAllText(rmqUserPath);
        //         rmqPass = System.IO.File.ReadAllText(rmqPassPath);
        //
        //         string certPath = Environment.GetEnvironmentVariable("CRTP");
        //         string keyPath = Environment.GetEnvironmentVariable("KP");
        //         string caPath = Environment.GetEnvironmentVariable("CAP");
        //
        //         key = System.IO.File.ReadAllText(keyPath);
        //         cert = System.IO.File.ReadAllText(certPath);
        //     }
        //     catch (Exception e)
        //     {
        //         Console.WriteLine(e);
        //     }
        //
        //     SslOption sslOption = new SslOption();
        //     sslOption.Enabled = true;
        //     sslOption.Certs = new X509Certificate2Collection(X509Certificate2.CreateFromPem(cert, key));
        //     sslOption.ServerName = "cluster.local";
        //
        //     ConnectionFactory factory = new ConnectionFactory();
        //     factory.UserName = rmqUser;
        //     factory.Password = rmqPass;
        //     factory.HostName = RabbitMqConstants.RabbitMqUri;
        //     factory.VirtualHost = "/";
        //     factory.Ssl = sslOption;
        //     
        //     IConnection connection = factory.CreateConnection();
        //     IModel model = connection.CreateModel();
        //     
        //
        //     _logger.LogInformation("Done");
        //
        //     RedirectToPage("/");
        // }

        public void OnPost()
        {
            var emailAddress = Request.Form["search"];
            _logger.LogInformation($"Form: {emailAddress}");
        
            _logger.LogInformation("Start");
            var source = new CancellationTokenSource(TimeSpan.FromSeconds(10));
            _bus.Publish<ExampleEmail>(new
            {
                email = emailAddress
            }, source.Token);
            
            _logger.LogInformation("Done");
        
            RedirectToPage("/");
        }
    }
}
