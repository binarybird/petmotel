namespace Common.Messaging
{
    public static class RabbitMqConstants
    {
        private const string RabbitMqUri = "petmotel-mq.petmotel.svc.cluster.local:5671";
        
        public const string JsonMimeType = "application/json";
        
        public const string CertPath = "/tls/tls.crt";
        public const string KeyPath = "/tls/tls.key";
        public const string RmqUserPath = "/rabbitmq/username";
        public const string RmqPassPath = "/rabbitmq/password";

        public static string GetRabbitMqUri(string user, string pass)
        {
            return $"amqp://{user}:{pass}@{RabbitMqUri}";
        }
    }
}