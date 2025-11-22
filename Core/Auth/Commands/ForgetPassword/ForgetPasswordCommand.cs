using MediatR;

namespace Application.Auth.Commands.ForgetPassword
{
    public record ForgetPasswordCommand(string Email):IRequest;
  
}
