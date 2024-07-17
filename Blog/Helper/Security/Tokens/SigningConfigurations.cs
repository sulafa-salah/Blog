using Microsoft.IdentityModel.Tokens;

namespace Blog.Helper.Security.Tokens
{
    public class SigningConfigurations
    {
        public string SecertKey { get; set; }
        public SecurityKey Key { get; }
        public SigningCredentials SigningCredentials { get; }

        public SigningConfigurations()
        {

            SecertKey = "ttttttttttttka16+fWsusfF/saic6MNCqT8oyaGUyc=";
            Key = new SymmetricSecurityKey(Convert.FromBase64String(SecertKey));
            SigningCredentials = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256);
        }
    }
}
