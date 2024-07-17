using FluentValidation;

namespace Blog.Contracts.Identity.Request
{
    public class UserRegistrationRequestDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }  
        
        

    #region   user registration Request Validation
    public class UserRegistrationRequestDtoValidation : AbstractValidator<UserRegistrationRequestDto>
    {
        public UserRegistrationRequestDtoValidation()
        {
            RuleFor(x => x.FirstName)
             .NotNull()
             .NotEmpty()
             .WithMessage("First Name is required.");
           

            RuleFor(x => x.LastName)
            .NotNull()
            .NotEmpty()
            .WithMessage("Last Name is required.");

            RuleFor(x => x.Email)
          .NotEmpty().WithMessage("Email address is required.")
          .EmailAddress().WithMessage("Invalid email address format.");

            RuleFor(x => x.Password)
           .NotEmpty().WithMessage("Password is required.")
           .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
           .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
           .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")
           .Matches("[0-9]").WithMessage("Password must contain at least one number.")
           .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character.");

            RuleFor(x => x.ConfirmPassword)
                .NotEmpty().WithMessage("Confirm password is required.")
                .Equal(x => x.Password).WithMessage("Confirm password does not match the password.");



        }
    }
    #endregion
}
