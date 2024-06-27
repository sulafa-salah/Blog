using Blog.Domain.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Blog.Domain.Enums;

namespace Blog.Persistence.Configuration
{
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.ToTable("Users");
            builder.Property(p => p.CreatedDate).HasColumnType("datetime").HasDefaultValueSql("getdate()");
            builder.Property(p => p.UpdatedDate).HasColumnType("datetime");
            builder.Property(p => p.PhoneNumber).HasMaxLength(12);
            
            builder.HasMany(e => e.UserRoles)
                .WithOne(e => e.User)
                .HasForeignKey(ur => ur.UserId)
                .OnDelete(DeleteBehavior.Restrict).IsRequired();

            builder.HasMany(e => e.RefreshTokens)
                .WithOne(e => e.User)
                .HasForeignKey(ur => ur.UserId)
                .OnDelete(DeleteBehavior.Restrict).IsRequired();

        }
        public class ApplicationRoleConfiguration : IEntityTypeConfiguration<ApplicationRole>
        {
            public void Configure(EntityTypeBuilder<ApplicationRole> builder)
            {
                builder.ToTable("Roles");
                // Each Role can have many entries in the UserRole join table
                builder.HasMany(e => e.UserRoles)
                    .WithOne(e => e.Role)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();
                builder.Property(p => p.Id).ValueGeneratedOnAdd();
                builder.Property(p => p.CreatedDate).HasColumnType("datetime").HasDefaultValueSql("getdate()");
               

                builder.HasData(
                   new ApplicationRole { Id = new Guid("A363EFAE-AF93-45D9-B900-94E4CF3AB610"), Name = ApplicationRoleEnums.Administrator.ToString(), NormalizedName = ApplicationRoleEnums.Administrator.ToString().ToUpper(), IsActive = true }

               );
            }
        }
        public class ApplicationUserRoleConfiguration : IEntityTypeConfiguration<ApplicationUserRole>
        {
            public void Configure(EntityTypeBuilder<ApplicationUserRole> builder)
            {
                builder.ToTable("UserRoles");
               
                builder.Property(p => p.CreatedBy).HasMaxLength(450);
                builder.HasKey(p => new { p.UserId, p.RoleId });
                builder.Property(p => p.CreatedDate).HasColumnType("datetime").HasDefaultValueSql("getdate()");
                builder.Property(p => p.Id).HasDefaultValueSql("NEWID()");
                builder.HasAlternateKey(x => new { x.Id }).HasName("UQ_UserRole_Id");
            }
        }
    }
}
