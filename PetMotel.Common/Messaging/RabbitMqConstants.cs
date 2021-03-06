namespace PetMotel.Common.Messaging
{
    public static class RabbitMqConstants
    {
        // public const string RabbitMqUri = "petmotel-mq.petmotel.svc.cluster.local";
        public const int Port = 5671;
        
        public const string JsonMimeType = "application/json";

        public const string IdentityService = "identity-service";
        
        // public const string CertPath = "/tls/tls.crt";
        // public const string KeyPath = "/tls/tls.key";
        // public const string RmqUserPath = "/rabbitmq/username";
        // public const string RmqPassPath = "/rabbitmq/password";

        public static string GetRabbitMqUri(string user, string pass, string rmqUri)
        {
            return $"amqp://{user}:{pass}@{rmqUri}:{Port}";
        }

        public static string GetServiceUri(string user, string pass, string service, string rmqUri)
        {
            return $"amqp://{user}:{pass}@{rmqUri}:{Port}/{service}";
        }
        
        public static string GetServiceUri(string service, string rmqUri)
        {
            return $"rabbitmq://{rmqUri}:{Port}/{service}";
        }
    }
}