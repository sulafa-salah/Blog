using FluentValidation;

namespace Blog.Contracts.Identity.Request
{
    public record UserLoginRequestDto(string Email, string Password);

    #region   user registration Request Validation
    public class UserLoginRequestDtoValidation : AbstractValidator<UserLoginRequestDto>
    {
        public UserLoginRequestDtoValidation()
        {
            

            RuleFor(x => x.Email)
          .NotEmpty().WithMessage("Email address is required.")
          .EmailAddress().WithMessage("Invalid email address format.");

            RuleFor(x => x.Password)
           .NotEmpty().WithMessage("Password is required.");
           

        }
    }
    #endregion
}
