using Application.Auth.Commands.Dtos;
using Application.Common.Email;
using Application.Contracts.EmailSender;
using Application.Contracts.Services;
using Application.DTOS;
using Application.Exceptions;
using AutoMapper;
using Infrastructure.IdentityEntities;
using MediatR;
using Microsoft.AspNetCore.Identity;


namespace Application.Auth.Commands.Register
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand,AuthDto>
    {
        private readonly ITokenService _tokenService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IBackgroundJobService _backgroundJobService;
        public RegisterCommandHandler(ITokenService tokenService,UserManager<AppUser> userManager
            , IMapper mapper, IBackgroundJobService backgroundJob)
        {
            _tokenService = tokenService;
            _userManager = userManager;
            _mapper = mapper;
            _backgroundJobService = backgroundJob;
        }
        public async Task<AuthDto> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            if (await _userManager.FindByEmailAsync(request.Email) != null)
                throw new BadRequestException("Email already exists");

            if (await _userManager.FindByNameAsync(request.UserName) != null)
                throw new BadRequestException("Username already exists");


            var user = _mapper.Map<AppUser>(request);

            //create a refresh token and access token in order to make the
            //user authenticated when he registers
            var refreshToken = _tokenService.CreateRefreshToken(user);
            
            user.RefreshTokens.Add(refreshToken);
           
            var roles =new List<string> { "User" }; // Default role for new users
            
            var token = _tokenService.CreateToken(user,roles);
            
            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                throw new BadRequestException(string.Join(",", result.Errors.Select(e => e.Description)));
            }

            //sending welcome email via hangfire
            _backgroundJobService.Enqueue<IEmailService>(x => x.SendWelcomeEmailAsync(user.UserName,user.Email));


            return new AuthDto
            {
                Token = token,
                RefreshToken = refreshToken.Token,
                UserName = user.UserName,
                Email = user.Email,
                Roles = roles.ToList(),
                RefreshTokenExpiration = refreshToken.ExpiryTime,
                UserId = user.Id,
                IsAuthenticated = true
            };


        }
    }
}
