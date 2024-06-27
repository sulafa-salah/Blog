using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Blog.Domain.Models;

namespace Blog.Persistence.Configuration
{
    public class CategoryConfiguartion : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
          
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id).HasColumnName("Id");

            builder.Property(p => p.Name)
                   .HasMaxLength(100);
            builder.Property(p => p.CreatedDate).HasColumnType("datetime").HasDefaultValueSql("getdate()");
            builder.Property(p => p.UpdatedDate).HasColumnType("datetime");
        }
    }
}
