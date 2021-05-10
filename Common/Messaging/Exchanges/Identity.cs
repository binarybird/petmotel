namespace Common.Messaging.Exchanges
{
    namespace Identity
    {
        public interface ILogin
        {
            string UserUuid { get; set; }
            string MessageUuid { get; set; }
            string UserName { get; set; }
            string Password { get; set; }
            bool RememberMe { get; set; }
        }

        public interface ILogOut
        {
        }

        public interface ICreate
        {
        }

        public interface IDelete
        {
        }

        public interface IUpdate
        {
        }

        public interface IIdentityReply
        {
            string UserUuid { get; set; }
            string MessageUuid { get; set; }
            int StatusCode { get; set; }
            bool Success { get; set; }
            string OriginatingMessageUuid { get; set; }
            string Message { get; set; }
        }
    }
}