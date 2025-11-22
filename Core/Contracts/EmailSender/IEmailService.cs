using Application.DTOS;

namespace Application.Contracts.EmailSender
{
    public interface IEmailService
    {
        Task SendEmailAsync(EmailDto request);
    }
}
