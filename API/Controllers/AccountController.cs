using Application.Auth.Commands.Address;
using Application.Auth.Commands.ForgetPassword;
using Application.Auth.Commands.Login;
using Application.Auth.Commands.Logout;
using Application.Auth.Commands.RefreshToken;
using Application.Auth.Commands.Register;
using Application.Auth.Commands.ResetPassword;
using Application.Auth.Queries.GetUserInfo;
using Infrastructure.IdentityEntities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly UserManager<AppUser> _userManager;
        public AccountController(IMediator mediator, UserManager<AppUser> userManager)
        {
            _mediator = mediator;
            _userManager = userManager;
        }
        [HttpPost("register")]
        public async Task<ActionResult> Register(RegisterCommand command)
        {

            var result = await _mediator.Send(command);
            return Ok(result);
        }


        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginCommand command)
        {
            var result = await _mediator.Send(command);
            //SetRefreshToken(result.RefreshToken,result.RefreshTokenExpiration);
            return Ok(result);
        }

        [Authorize]
        [HttpGet("userinfo")]
    
        public async Task<ActionResult> GetUserInfo([FromQuery] bool withAddress)
        {

            var result = await _mediator.Send(new GetUserInfoQuery { WithAddress= withAddress });
            return Ok(result);

        }

        [HttpPost("logout")]
        public async Task<ActionResult> Logout(LogoutCommand command)
        {
            await _mediator.Send(command);
            return Ok();
        }

        [HttpPost("address")]
        public async Task<ActionResult> CreateOrUpdateAddress(CreateOrUpdateAddressCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        
        [HttpPost("refresh-token")]
        public async Task<ActionResult> RefreshToken(RefreshTokenCommand command)
        {
          //var refreshToken = Request.Cookies["refreshtoken"];
            var result = await _mediator.Send(command);
            //SetRefreshToken(result.RefreshToken,result.RefreshTokenExpiration);
            return Ok(result);

        }

        [HttpPost("Forget-Password")]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordCommand command)
        {
           await _mediator.Send(command);

           return Ok();
        }

        [HttpPost("Reset-Password")]
        public async Task<IActionResult> ResestPassword(ResetPasswordCommand command)
        {
            await _mediator.Send(command);

            return Ok();
        }


        //private void SetRefreshToken(string refreshToken, DateTime expires)
        //{
        //    var cookieOptions = new CookieOptions
        //    {
        //        HttpOnly = true,
        //        Secure = true,
        //        SameSite = SameSiteMode.None,
        //        Expires = expires.ToLocalTime()
        //    };
        //    Response.Cookies.Append("refreshtoken", refreshToken, cookieOptions);
        //}
    }
}
