using Blog.Domain.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Blog.Persistence.Configuration
{
 
    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id).HasColumnName("Id");

          
            builder.Property(p => p.CreatedDate).HasColumnType("datetime").HasDefaultValueSql("getdate()");
            builder.Property(p => p.UpdatedDate).HasColumnType("datetime");

            builder.HasOne(p => p.Post).WithMany(p => p.Comments).HasForeignKey(p => p.PostId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(p => p.User).WithMany(p => p.Comments).HasForeignKey(p => p.CreatedBy).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(p => p.User).WithMany(p => p.Comments).HasForeignKey(p => p.UpdatedBy).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
