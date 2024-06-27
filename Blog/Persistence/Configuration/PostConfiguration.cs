using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Blog.Domain.Models;

namespace Blog.Persistence.Configuration
{
    public class PostConfiguration : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id).HasColumnName("Id");

            builder.Property(p => p.Title)
                   .HasMaxLength(100);

            builder.Property(p => p.CreatedDate).HasColumnType("datetime").HasDefaultValueSql("getdate()");
            builder.Property(p => p.UpdatedDate).HasColumnType("datetime");
            builder.HasOne(p => p.Category).WithMany(p => p.Posts).HasForeignKey(p => p.CategoryId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(p => p.User).WithMany(p => p.Posts).HasForeignKey(p => p.CreatedBy).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(p => p.User).WithMany(p => p.Posts).HasForeignKey(p => p.UpdatedBy).OnDelete(DeleteBehavior.Restrict);


        }
    }
}
