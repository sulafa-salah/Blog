using Azure.Core;
using Blog.Contracts.Common.Response;
using Microsoft.Extensions.Localization;

namespace Blog.Contracts.Auth.Response
{
    public record TokenResponse : ResponseDto
    {



        public string? Username { get; set; }
        public string? Email { get; set; }
        // public List<string>? Roles { get; set; }
        public string? Token { get; set; }
        //  public DateTime? ExpiresOn { get; set; }

        public string? RefreshToken { get; set; }

        public DateTime RefreshTokenExpiration { get; set; }


    }
}
