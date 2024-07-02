using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;

namespace Blog.Domain.Models
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public ApplicationUser()
        {
         Id = Guid.NewGuid();
         RefreshTokens= new HashSet<RefreshToken>();
            Comments=new HashSet<Comment>();
            Posts=new HashSet<Post>();
            UserRoles=new HashSet<ApplicationUserRole>();
            IsActive = true;
        }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public Guid? UpdatedBy { get; set; }
        public bool IsActive { get; set; }     
      

    
        public ICollection<ApplicationUserRole> UserRoles { get; set; }
        public ICollection<Post> Posts { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<RefreshToken> RefreshTokens { get; set; }



    }

    public class ApplicationRole : IdentityRole<Guid>
    {
        public ApplicationRole()
        {
            Id = Guid.NewGuid();

        }
      
        public string? Description { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; }
        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }
       
        
    }

    public class ApplicationUserRole : IdentityUserRole<Guid>
    {
        public ApplicationUserRole()
        {
            Id = Guid.NewGuid();
        }
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid? CreatedBy { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual ApplicationRole Role { get; set; }
    }
    public class ApplicationUserClaim : IdentityUserClaim<Guid>
    {
        public ApplicationUserClaim()
        {
            Id = Guid.NewGuid();
        }
        public Guid Id { get; set; }
        public ApplicationUser User { get; set; }
        public Guid CreatedBy { get; set; }

    }

    public class ApplicationRoleClaim : IdentityRoleClaim<Guid>
    {
        public ApplicationRoleClaim()
        {
            Id = Guid.NewGuid();
        }
        public Guid Id { get; set; }
        public int ClaimId { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid? CreatedBy { get; set; }
      
        public virtual ApplicationRole Role { get; set; }
    }

    public class ApplicationUserLogin : IdentityUserLogin<Guid>
    {
        public ApplicationUserLogin()
        {
            Id = Guid.NewGuid();

        }
        public Guid Id { get; set; }
        public ApplicationUser User { get; set; }
        public Guid CreatedBy { get; set; }
    }

    public class ApplicationUserToken : IdentityUserToken<Guid>
    {
        public ApplicationUserToken()
        {
            Id = Guid.NewGuid();

        }
        public Guid Id { get; set; }
        public ApplicationUser User { get; set; }
        public Guid CreatedBy { get; set; }
    }
   

}