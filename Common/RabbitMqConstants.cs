namespace Common
{
    public static class RabbitMqConstants
    {
        // public const string RabbitMqUri = "amqp://guest:guest@production-rabbitmqcluster-nodes.default.svc.cluster.local";
        // public const string RabbitMqUri = "amqp://service:service12345@petmotel-mq.petmotel.svc.cluster.local:5672";
        // public const string RabbitMqUri = "amqp://service:service12345@petmotel-mq.petmotel.svc.cluster.local:5672";
        private const string RabbitMqUri = "petmotel-mq.petmotel.svc.cluster.local:5672";
        public const string JsonMimeType = "application/json";

        public const string IdentityExchange = "petmotel.identity.exchange";
        public const string BasketExchange = "petmotel.basket.exchange";

        public const string IdentityLoginLogoutQueue = "petmotel.login.logout.queue";
        public const string IdentityAccountQueue = "petmotel.account.queue";

        public static string GetRabbitMqUri(string user, string pass)
        {
            return $"amqp://{user}:{pass}@{RabbitMqUri}";
        }
    }
}