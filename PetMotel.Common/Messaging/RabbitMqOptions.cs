namespace PetMotel.Common.Messaging
{
    public class RabbitMqOptions
    {
        public const string RabbitMq = "RabbitMq";

        public RabbitMqOptions()
        {
            
        }
        
        public string Uri { get; set; }
        public string CertPath { get; set; }
        public string KeyPath { get; set; }
        public string UserPath { get; set; }
        public string PassPath { get; set; }
    }
}