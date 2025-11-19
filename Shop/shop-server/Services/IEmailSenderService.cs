using System.Threading.Tasks;

namespace MusicShop.Services
{
    public interface IEmailSenderService
    {
        Task SendEmailAsync(string receiverEmail, string subject, string message);
    }
}

