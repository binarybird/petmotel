using System;
using System.IO;

namespace PetMotel.Common.Messaging
{
    public class RmqInitializer
    {
        public static (string cert, string key, string rmqUser, string rmqPass) Initialize()
        {
            string cert = File.ReadAllText(RabbitMqConstants.CertPath);
            string key = File.ReadAllText(RabbitMqConstants.KeyPath);
            string rmqUser = File.ReadAllText(RabbitMqConstants.RmqUserPath);
            string rmqPass = File.ReadAllText(RabbitMqConstants.RmqPassPath);

            if (String.IsNullOrEmpty(cert))
            {
                throw new Exception("Unable to read certificate");
            }

            if (String.IsNullOrEmpty(key))
            {
                throw new Exception("Unable to read certificate key");
            }

            if (String.IsNullOrEmpty(rmqUser))
            {
                throw new Exception("Unable to read rmq user");
            }

            if (String.IsNullOrEmpty(rmqPass))
            {
                throw new Exception("Unable to read rmq password");
            }

            return (cert, key, rmqUser, rmqPass);
        }
    }
}