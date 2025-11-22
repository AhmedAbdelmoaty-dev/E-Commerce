
using Application.Exceptions;
using Infrastructure.IdentityEntities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Auth.Commands.ResetPassword
{
    public class ResetPasswordCommandHandler:IRequestHandler<ResetPasswordCommand>
    {
        private readonly UserManager<AppUser> _userManager;
        public ResetPasswordCommandHandler(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task Handle(ResetPasswordCommand command, CancellationToken cancellationToken)
        {
          var user= await  _userManager.FindByEmailAsync(command.Email);
            
            if (user is null)
                return;

            var decodedToken = Uri.UnescapeDataString(command.Token);

            var result= await _userManager.ResetPasswordAsync(user, decodedToken, command.NewPassword);

            if (!result.Succeeded)
            {
              var errorsDescriptions=  result.Errors.Select(x => x.Description);

              var errorMessage=  string.Join(",", errorsDescriptions);

                throw new BadRequestException(errorMessage);

            }

           
        }
    }
}
