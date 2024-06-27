namespace Blog.Contracts.Identity.Request
{
    public record UserRegistrationRequestDto(   string FirstName, string LastName,string Email,string Password,string ConfirmPassword);
           
    
}
