namespace Common
{
    public static class RabbitMqConstants
    {
        public const string RabbitMqUri = "amqp://guest:guest@production-rabbitmqcluster-nodes.default.svc.cluster.local";

        public const string JsonMimeType = "application/json";

        public const string TestExchange = "petmotel.test.exchange";

        public const string TestQueue = "petmotel.test.queue";

    }
}