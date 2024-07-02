using Blog.Domain.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Blog.Persistence.Configuration
{
    public class EmailLoggerConfiguration : IEntityTypeConfiguration<EmailLogger>
    {
        public void Configure(EntityTypeBuilder<EmailLogger> builder)
        {

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id).HasColumnName("Id").HasDefaultValueSql("newId()");

            builder.Property(p => p.Email)
                       .HasMaxLength(100);

            builder.Property(p => p.EmailSent).HasColumnType("datetime").HasDefaultValueSql("getdate()");
        }
    }
}
