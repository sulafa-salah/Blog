using Azure.Core;
using Blog.Contracts.Auth.Response;
using Blog.Domain.Models;
using Blog.Options;
using Blog.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace Blog.Helper.Security.Tokens
{
    public interface ITokenHandler
    {
        Task<TokenResponse> CreateAccessToken(ApplicationUser user);
     
    }
    public class TokenHandler : ITokenHandler
    {
       
        private readonly TokenOption _tokenOptions;
        private readonly SigningConfigurations _signingConfigurations;
        private IUnitOfWork unitOfWork;
        private readonly UserManager<ApplicationUser> userManager;

        private readonly RoleManager<ApplicationRole> roleManager;
        public TokenHandler(IOptions<TokenOption> tokenOptionSnapshot, SigningConfigurations signingConfigurations, IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            _tokenOptions = tokenOptionSnapshot.Value;
          
            this.unitOfWork = unitOfWork;
            _signingConfigurations = signingConfigurations;
            this.userManager = userManager;

            this.roleManager = roleManager;
        }
        public async Task<TokenResponse> CreateAccessToken(ApplicationUser user)
        {
            var tokenResponse = new TokenResponse();
         
            var jwtSecurityToken = await BuildAccessToken(user);
            tokenResponse.IsSuccess = true;
            tokenResponse.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            tokenResponse.Email = user.Email;
            tokenResponse.Username = user.UserName;
            // tokenResponse.ExpiresOn = jwtSecurityToken.ValidTo;
            var userTokens = await userManager.Users.Include(u => u.RefreshTokens).ToListAsync();
        


            if (userTokens.LastOrDefault().RefreshTokens.Any(x=> x.IsActive))
            {
                var activeRefreshToken = user.RefreshTokens.FirstOrDefault(t => t.IsActive);
                tokenResponse.RefreshToken = activeRefreshToken.Token;
                tokenResponse.RefreshTokenExpiration = activeRefreshToken.ExpiresOn;
            }
            else
            {
                var refreshToken = BuildRefreshToken();
                tokenResponse.RefreshToken = refreshToken.Token;
                tokenResponse.RefreshTokenExpiration = refreshToken.ExpiresOn;
                user.RefreshTokens.Add(refreshToken);
                await userManager.UpdateAsync(user);
            }

            return tokenResponse;

           
        }
        private async Task<JwtSecurityToken> BuildAccessToken(ApplicationUser user)
        {
           
            var accessTokenExpiration = DateTime.UtcNow.AddMinutes(_tokenOptions.AccessTokenExpiration);

            var securityToken = new JwtSecurityToken
            (
                issuer: _tokenOptions.Issuer,
                audience: _tokenOptions.Audience,
                claims: await GetClaims(user),
                expires: accessTokenExpiration,
             
                signingCredentials: _signingConfigurations.SigningCredentials
            );
         //   var accessToken = new JwtSecurityTokenHandler().WriteToken(securityToken);
            


            return securityToken;
        }
        private RefreshToken BuildRefreshToken()
        {
            var randomNumber = new byte[32];

            using var generator = new RNGCryptoServiceProvider();

            generator.GetBytes(randomNumber);

            return new RefreshToken
            {
                Token = Convert.ToBase64String(randomNumber),
                ExpiresOn = DateTime.UtcNow.AddDays(10),
                CreatedOn = DateTime.UtcNow
            };
        }
        private async Task<IEnumerable<Claim>> GetClaims(ApplicationUser user)
        {
            var roles = await userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),

                // Add your custom claims here 
                  };
            if (roles.Any())
            {
                foreach (var userRole in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, userRole));
                }
            }
      

           
            return claims;
        }


    }
}
