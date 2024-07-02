using Blog.Contracts;
using Blog.Contracts.Auth.Response;
using Blog.Contracts.BackgroundServices;
using Blog.Contracts.Common.Response;
using Blog.Contracts.Identity.Request;
using Blog.Domain.Models;
using Blog.Helper.Security.Tokens;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System.IdentityModel.Tokens.Jwt;
using System.Net;

namespace Blog.Services
{
    public interface IAuthService
    {
        Task<ResponseDto> CreateUserAsync(UserRegistrationRequestDto request);
        Task<TokenResponse> AuthenticateAsync(UserLoginRequestDto request);
        Task<TokenResponse> RefreshTokenAsync(string token);
        Task<bool> RevokeTokenAsync(string token);
    }
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IStringLocalizer<AuthService> localizer;
        private readonly ITokenHandler tokenHandler;
        private readonly IBus bus;

        public AuthService(UserManager<ApplicationUser> userManager, IStringLocalizer<AuthService> localizer, ITokenHandler tokenHandler, IBus bus)
        {
            this.userManager = userManager;
            this.localizer = localizer;
            this.tokenHandler = tokenHandler;
            this.bus = bus;
        }

        public async Task<ResponseDto> CreateUserAsync(UserRegistrationRequestDto request)
        {
            // check existing User
            var userExist = await userManager.FindByEmailAsync(request.Email);
            if (userExist != null)
            {
                return new ResponseDto { IsSuccess = false, ResponseMessage = localizer["UserWithEmailExists"], ResponseCode = (int)HttpStatusCode.BadRequest };
            }

            // Construct user object
            var user = new ApplicationUser { FirstName = request.FirstName, LastName = request.LastName, UserName = request.Email, Email = request.Email };

            // Create user 
            var result = await userManager.CreateAsync(user, request.Password);

            // Construct and return user creation error response
            if (!result.Succeeded)
            {
                return new ResponseDto { IsSuccess = false, ResponseMessage = localizer["RegisterUserResponseErrMsg"], };
            }
            var endpoint = await bus.GetSendEndpoint(new Uri("queue:" + AppConst.RMQueues.UserRegistrationMQ));
            await endpoint.Send(new EmailMessage
            {
                Email = user.Email,
               
            });
            return new ResponseDto { IsSuccess = true, ResponseMessage = localizer["RegisterAccountSentVerificationSuccessMsg"], ResponseCode = (int)HttpStatusCode.OK };

        }
        public async Task<TokenResponse> AuthenticateAsync(UserLoginRequestDto request)
        {
            var user = await userManager.FindByEmailAsync(request.Email);
            if (user != null)
            {
                // validate the password.
                if (!await userManager.CheckPasswordAsync(user, request.Password))
                {
                    return new TokenResponse
                    {
                        IsSuccess = false,
                        ResponseCode = ApiStatusCode.BadRequest,
                        ResponseMessage = localizer["LoginResponseInvalidEmailPasswordErrMsg"]
                    };
                }

                // Check user activation.
                if (!user.IsActive)
                {
                    return new TokenResponse
                    {
                        IsSuccess = false,
                        ResponseCode = ApiStatusCode.BadRequest,
                        ResponseMessage = localizer["LoginResponseDisabledAccountErrMsg"]
                    };
                }
                // Generate access token.
                var token = await tokenHandler.CreateAccessToken(user);
                return token;
            }
            else
            {
                return new TokenResponse
                {
                    IsSuccess = false,
                    ResponseCode = ApiStatusCode.BadRequest,
                    ResponseMessage = localizer["LoginResponseUnauthorizedUserErrMsg"]
                };
            }
        }

        public async  Task<TokenResponse> RefreshTokenAsync(string token)
        {
            var tokenResponse = new TokenResponse();

            var user = await userManager.Users.SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == token));

            if (user == null)
            {
                tokenResponse.ResponseMessage = "Invalid token";
                return tokenResponse;
            }

            var refreshToken = user.RefreshTokens.Single(t => t.Token == token);

            if (!refreshToken.IsActive)
            {
                tokenResponse.ResponseMessage = "Inactive token";
                return tokenResponse;
            }

            refreshToken.RevokedOn = DateTime.UtcNow;

            tokenResponse =await tokenHandler.CreateAccessToken(user);

            return tokenResponse;
        }
        public async Task<bool> RevokeTokenAsync(string token)
        {
            var user = await userManager.Users.SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == token));

            if (user == null)
                return false;

            var refreshToken = user.RefreshTokens.Single(t => t.Token == token);

            if (!refreshToken.IsActive)
                return false;

            refreshToken.RevokedOn = DateTime.UtcNow;

            await userManager.UpdateAsync(user);

            return true;
        }
    }
}
