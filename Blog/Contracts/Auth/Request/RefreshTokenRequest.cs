namespace Blog.Contracts.Auth.Request
{
    public record RefreshTokenRequest (string RefreshToken);
    public record RevokeTokenRequest(string RevokeToken);


}
