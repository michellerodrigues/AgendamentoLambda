
    public class EmailConfigOptions//: IEmailConfigOptions
    {
        public const string EmailConfig = "EmailConfig";
        public EmailOptions Credentials { get; set; }
        public ServerOptions Server { get; set; }

        //public EmailConfigOptions GetEmailConfigOptions()
        //{
        //    return new EmailConfigOptions();
        //}
    }

    public class EmailOptions
    {
        public const string Credentials = "Credentials";
        public string Username { get; set; }
        public string Password { get; set; }
    }
    public class ServerOptions
    {
        public const string SMTPServer = "SMTPServer";
        public string Address { get; set; }
        public int Port { get; set; }
    }

