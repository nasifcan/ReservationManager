using MailKit.Net.Smtp;
using MimeKit;
namespace RezervationSystem.Services
{
    public class EMailSenderService
    {
        public string fromName = "";
        public string fromMail = "";
        public string fromPass = "";
        public async Task<string> SendEmailAsync(string toEmail, string subject, string body, string toName, string fromEmail, string fromPass, string fromName)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(fromName, fromEmail));
                message.To.Add(new MailboxAddress(toName, toEmail));
                message.Subject = subject;

                message.Body = new TextPart("plain")
                {
                    Text = body
                };

                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync("smtp.example.com", 587, false);
                    await client.AuthenticateAsync(fromEmail, fromPass);
                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                }
                return "OK";
            }
            catch (Exception ex)
            {
                return $"HATA SendEmailAsync: {ex.Message}";
            }
        }

    }
}
