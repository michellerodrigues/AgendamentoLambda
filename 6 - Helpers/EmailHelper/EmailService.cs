using System;
using System.Net;
using System.Net.Mail;

namespace EmailHelper
{
    public class EmailService: IEmailService
    { 

        private readonly SmtpClient _smtpClient;
        private readonly MailAddress _mailAddress;


        public EmailService(EmailConfigOptions emailConfigOptions)
        {
            _smtpClient = new SmtpClient(emailConfigOptions.Server.Address, emailConfigOptions.Server.Port)
            {
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(emailConfigOptions.Credentials.Username, emailConfigOptions.Credentials.Password),
                EnableSsl = true
            };

            _mailAddress = new MailAddress(emailConfigOptions.Credentials.Username);


        }

        public bool Enviar(string remetente, string assunto, string mensagem)
        {
            try
            {

                MailMessage mail = new MailMessage();
                mail.From = _mailAddress;
                mail.To.Add(remetente);
                mail.Subject = String.Format("AgroPop informa: {0}", assunto);

                mail.Body = mensagem;

                mail.IsBodyHtml = true;
                _smtpClient.Send(mail);

                return true;

            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Exception in sendEmail:" + ex.Message);
            }
        }
    }
}
