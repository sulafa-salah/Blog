namespace Blog.Contracts.Auth.Response
{
    public record AuthenticationResult
      (string Token,
        bool Success,
        string ErrorMessage
        );
}
