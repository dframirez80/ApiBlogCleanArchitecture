using Domain.Models;
using System.Threading.Tasks;

namespace Domain.Email
{
    public interface IMailService
    {
        Task SendEmailAsync(EmailRequest emailRequest);
        Task EmailChangePassword(EmailRequest emailRequest);
        Task EmailConfirmRegister(EmailRequest emailRequest);

    }

}
