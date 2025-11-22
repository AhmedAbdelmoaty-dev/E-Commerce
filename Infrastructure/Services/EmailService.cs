using Application.Contracts.EmailSender;
using Application.DTOS;
using Infrastructure.Options;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;


namespace Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailOptions _settings;
        public EmailService(IOptions<EmailOptions> settings)
        {
            _settings = settings.Value;
        }
        public async Task SendEmailAsync(EmailDto request)
        {
            var email = new MimeMessage();

            email.From.Add(new MailboxAddress(_settings.From,_settings.Email));
            
            email.To.Add(MailboxAddress.Parse(request.to));

            email.Subject = request.subject;

            email.Body = new TextPart(TextFormat.Html) { Text=request.body};

            using var smtp = new SmtpClient();

           await  smtp.ConnectAsync(_settings.Host,_settings.Port,MailKit.Security.SecureSocketOptions.StartTls);

           await  smtp.AuthenticateAsync(_settings.Email,_settings.Password);

           await  smtp.SendAsync(email);

            await smtp.DisconnectAsync(true);


        }
    }
}
