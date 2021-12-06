using System;
using System.IO;

namespace PetMotel.Common.Messaging
{
    public class RmqInitializer
    {
        public static (string cert, string key, string rmqUser, string rmqPass) Initialize(RabbitMqOptions options)
        {
            string cert = File.ReadAllText(options.CertPath);
            string key = File.ReadAllText(options.KeyPath);
            string rmqUser = File.ReadAllText(options.UserPath);
            string rmqPass = File.ReadAllText(options.PassPath);

            if (String.IsNullOrEmpty(cert))
            {
                throw new System.Exception("Unable to read certificate");
            }

            if (String.IsNullOrEmpty(key))
            {
                throw new System.Exception("Unable to read certificate key");
            }

            if (String.IsNullOrEmpty(rmqUser))
            {
                throw new System.Exception("Unable to read rmq user");
            }

            if (String.IsNullOrEmpty(rmqPass))
            {
                throw new System.Exception("Unable to read rmq password");
            }

            return (cert, key, rmqUser, rmqPass);
        }
    }
}