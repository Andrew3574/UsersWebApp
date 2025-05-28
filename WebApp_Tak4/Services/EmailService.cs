using MailKit.Net.Smtp;
using MimeKit;

namespace WebApp_Task4.Services
{
    public class EmailService
    {
        public EmailService() { }
        public async Task SendAsync(string email, string subject, string message)
        {
            using var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("Администрация сайта Task4", "7heproffi123@gmail.com"));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart("plain")
            {
                Text = message
            };

            using (var client = new SmtpClient())
            {
                try
                {
                    await client.ConnectAsync("smtp.gmail.com", 465, MailKit.Security.SecureSocketOptions.SslOnConnect);
                    await client.AuthenticateAsync("7heproffi123@gmail.com", "boxx dwli imfh zkwq");
                    await client.SendAsync(emailMessage);
                }
                catch (Exception)
                {

                    throw;
                }
                finally
                {
                    await client.DisconnectAsync(true);
                }
            }
        }

        public async Task<string> SendRecoveryCode(string email)
        {
            var code = GetRecoveryCode();
            await SendAsync(email,"Recovery code", $"Your recovery code: {code}");
            return code;
        }

        private string GetRecoveryCode()
        {
            var code = new Random().Next(1000, 9999).ToString();
            return code;
        }
    }
}
