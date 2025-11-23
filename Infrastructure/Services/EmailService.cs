using Application.Common.Email;
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
        private readonly IEmailTemplateService _emailTemplate;
        public EmailService(IOptions<EmailOptions> settings,IEmailTemplateService emailTemplate)
        {
            _settings = settings.Value;
            _emailTemplate  = emailTemplate;
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

        public async Task SendForgetPasswordAsync(string UserName, string email, string ResetLink)
        {
            var body = _emailTemplate.GetForgetPasswordTemplate(UserName, ResetLink);

            var emailDto = new EmailDto(email, "Forget Password", body);

            await SendEmailAsync(emailDto);
        }

        public async Task SendWelcomeEmailAsync(string UserName, string email)
        {
           

            var body = _emailTemplate.GetWelcomeEmailTemplate(UserName);

            var emailDto = new EmailDto(email, "Welcome to  E-Commerce app", body);

            await SendEmailAsync(emailDto);
        }
    }
}
