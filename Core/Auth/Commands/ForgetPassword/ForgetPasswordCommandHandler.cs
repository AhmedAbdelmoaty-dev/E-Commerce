using Application.Common.Email;
using Application.Contracts.EmailSender;
using Application.Contracts.Services;
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
        private readonly UserManager<AppUser> _userManager;
        private readonly IOptions<FrontendOptions> _frontendOptions;
        private readonly IBackgroundJobService _backgroundJobService;

        public ForgetPasswordCommandHandler(UserManager<AppUser> userManager,IOptions<FrontendOptions> options,
            IBackgroundJobService backgroundJob)
        {
            _userManager=userManager;
            _frontendOptions = options;
            _backgroundJobService=backgroundJob;
        }
        public async Task Handle(ForgetPasswordCommand request, CancellationToken cancellationToken)
        {
          var user=  await _userManager.FindByEmailAsync(request.Email);

            if (user is null)
                return;

          var token=  await _userManager.GeneratePasswordResetTokenAsync(user);

          var encodedToken = Uri.EscapeDataString(token);

          var frontendUrl = _frontendOptions.Value.ResetPasswordUrl;

          var resetLink = $"{frontendUrl}?email={request.Email}&token={encodedToken}";


            _backgroundJobService.Enqueue<IEmailService>(x =>x.SendForgetPasswordAsync(user.UserName,user.Email, resetLink));

        }
    }
}
