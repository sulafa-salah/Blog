using Blog.Contracts.Identity.Request;
using Blog.Contracts;
using Blog.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Azure.Core;
using Blog.Contracts.Auth.Request;

namespace Blog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService authService;

        public AuthController(IAuthService authService)
        {
            this.authService = authService;
        }

        [HttpPost(ApiRoute.Auth.Register)]

        public async Task<IActionResult> Register([FromBody] UserRegistrationRequestDto request)
        {
            var result = await authService.CreateUserAsync(request);
            return StatusCode(result.ResponseCode, result);
        }

        [HttpPost(ApiRoute.Auth.Login)]
        public async Task<IActionResult> Login([FromBody] UserLoginRequestDto userCredentials)
        {
            var response = await authService.AuthenticateAsync(userCredentials);
            if (!response.IsSuccess)
            {
                return BadRequest(response.ResponseMessage);
            }

            return Ok(response);
        }

        [HttpGet(ApiRoute.Auth.RefreshToken)]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {

            if (string.IsNullOrEmpty(request.RefreshToken))
            {
                return BadRequest("Refresh token is required.");
            }


            var result = await authService.RefreshTokenAsync(request.RefreshToken);

            if (!result.IsSuccess)
                return BadRequest(result);

           

            return Ok(result);
        }
        [HttpPost(ApiRoute.Auth.RevokeToken)]
        
        public async Task<IActionResult> RevokeToken([FromBody] RevokeTokenRequest request)
        {
           

            if (string.IsNullOrEmpty(request.RevokeToken))
                return BadRequest("Token is required!");

            var result = await authService.RevokeTokenAsync(request.RevokeToken);

            if (!result)
                return BadRequest("Token is invalid!");

            return Ok();
        }
    }
}
