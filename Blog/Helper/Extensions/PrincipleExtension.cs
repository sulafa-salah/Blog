using System.IdentityModel.Tokens.Jwt;

namespace Blog.Helper.Extensions
{
 
     public static class PrincipleExtension
    {
        /// <summary>
        /// Get ApplicationUserId from Claims.
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public static string GetUserId(this HttpContext httpContext)
        {
            if (httpContext == null)
            {
                return string.Empty;
            }
            return httpContext.User.Claims.SingleOrDefault(s => s.Type == JwtRegisteredClaimNames.Jti)?.Value;
        }

    }
}
