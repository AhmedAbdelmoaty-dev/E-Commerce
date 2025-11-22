using Application.Contracts.EmailSender;
using Application.DTOS;
using Application.Options;
using Infrastructure.IdentityEntities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;


namespace Application.Auth.Commands.ForgetPassword
{
    public class ForgetPasswordCommandHandler : IRequestHandler<ForgetPasswordCommand>
    {

        private readonly IEmailService _emailService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IOptions<FrontendOptions> _frontendOptions;
        public ForgetPasswordCommandHandler(IEmailService emailService
            ,UserManager<AppUser> userManager,IOptions<FrontendOptions> options)
        {
            _emailService=emailService;
            _userManager=userManager;
            _frontendOptions = options;
        }
        public async Task Handle(ForgetPasswordCommand request, CancellationToken cancellationToken)
        {
          var user=  await _userManager.FindByEmailAsync(request.Email);

            if (user is null)
                return;

          var token=  await _userManager.GeneratePasswordResetTokenAsync(user);

          var encodedToken = Uri.EscapeDataString(token);

          var frontendUrl = _frontendOptions.Value.ResetPasswordUrl;

          var url = $"{frontendUrl}?email={request.Email}&token={encodedToken}";

          var body = $@"
            <h2>Reset Your Password</h2>
            <p>Click the link below to reset your password:</p>
            <a href='{url}'>Reset Password</a>";

            var emailDto = new EmailDto(request.Email, "Reset Password", body);

            await _emailService.SendEmailAsync(emailDto);

        }
    }
}
