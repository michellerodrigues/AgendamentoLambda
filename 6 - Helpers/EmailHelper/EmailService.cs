using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace EmailHelper
{
    public class EmailService: IEmailService
    { 

        private readonly SmtpClient _smtpClient;
        private readonly MailAddress _mailAddress;
        private readonly EmailConfigOptions _emailConfigOptions;

        public EmailService(EmailConfigOptions emailConfigOptions)
        {
            _emailConfigOptions = emailConfigOptions;

            _mailAddress = new MailAddress(_emailConfigOptions.Credentials.Username);

            _smtpClient = new System.Net.Mail.SmtpClient(_emailConfigOptions.Server.Address, _emailConfigOptions.Server.Port)
            {
                // Pass SMTP credentials
                Credentials =
                    new NetworkCredential(_emailConfigOptions.Credentials.Username, _emailConfigOptions.Credentials.Password),

                // Enable SSL encryption
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Timeout= 20_000
            };


        }

        
        public async Task<bool> Enviar(string remetente, string assunto, string mensagem)
        {
            try
            {
                

                MailMessage mail = new MailMessage();
                mail.From = _mailAddress;
                mail.To.Add(remetente);
                mail.Subject = String.Format("AgroPop informa: {0}", assunto);

                mail.Body = mensagem;

                mail.IsBodyHtml = true;
                await _smtpClient.SendMailAsync(mail).ConfigureAwait(false);

                return true;

            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Exception in sendEmail:" + ex.Message + " | "+ ex.StackTrace);
            }
        }
    }
}
