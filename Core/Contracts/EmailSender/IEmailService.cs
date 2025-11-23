using Application.DTOS;

namespace Application.Contracts.EmailSender
{
    public interface IEmailService
    {
        Task SendEmailAsync(EmailDto request);

        Task SendWelcomeEmailAsync(string UserName,string email);

        Task SendForgetPasswordAsync(string UserName,string email,string ResetLink);
    }
}
