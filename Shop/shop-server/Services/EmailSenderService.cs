using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace MusicShop.Services
{
    public class EmailSenderService : IEmailSenderService
    {
        private readonly IConfiguration _configuration;

        public EmailSenderService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string receiverEmail, string subject, string body)
        {
            var smtpSettings = _configuration.GetSection("SmtpSettings");
            var server = smtpSettings["Server"];
            var port = int.Parse(smtpSettings["Port"]!);
            var senderEmail = smtpSettings["SenderEmail"];
            var senderName = smtpSettings["SenderName"];
            var username = smtpSettings["Username"];
            var password = smtpSettings["Password"];

            using (var client = new SmtpClient(server, port))
            {
                client.Credentials = new NetworkCredential(username, password);
                client.EnableSsl = true;

                var message = new MailMessage
                {
                    From = new MailAddress(senderEmail, senderName),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };
                message.To.Add(receiverEmail);

                await client.SendMailAsync(message);
            }
        }
    }
}