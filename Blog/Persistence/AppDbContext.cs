using Blog.Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;

namespace Blog.Persistence
{
    public class AppDbContext  : IdentityDbContext<
        ApplicationUser, ApplicationRole, Guid,
        ApplicationUserClaim, ApplicationUserRole, ApplicationUserLogin,
        ApplicationRoleClaim, ApplicationUserToken>
    {

        private readonly IHttpContextAccessor _httpContextAccessor;
        public AppDbContext(IHttpContextAccessor httpContextAccessor, DbContextOptions<AppDbContext> options)
            : base((DbContextOptions)options)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Category> Categories { get; set; }
   
        public DbSet<Comment> Comments { get; set; }

        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            var AddedEntities = ChangeTracker.Entries()
                .Where(E => E.State == EntityState.Added)
                .ToList();
            var userId = _httpContextAccessor?.HttpContext.User.Claims.SingleOrDefault(s => s.Type == JwtRegisteredClaimNames.Jti)?.Value;

            AddedEntities.ForEach(E =>
            {
                E.Property("CreatedBy").CurrentValue =  new Guid(userId);
            });


            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("data source=.;initial catalog=Blog;user id=sa; password=admin;MultipleActiveResultSets=True;");

                    }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        }
    
}
}
