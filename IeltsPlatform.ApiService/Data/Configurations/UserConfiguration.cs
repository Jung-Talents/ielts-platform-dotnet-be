using IeltsPlatform.ApiService.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IeltsPlatform.ApiService.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);

            builder.Property(u => u.Id)
                .HasColumnName("id");

            builder.Property(u => u.AvatarLink)
                .HasColumnName("avatar_link")
                .IsRequired();

            builder.Property(u => u.Email)
                .HasColumnName("email")
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(u => u.Username)
                .HasColumnName("username")
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(u => u.PhoneNumber)
                .HasColumnName("phone_number")
                .HasMaxLength(20);

            builder.Property(u => u.Password)
                .HasColumnName("password")
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(u => u.Role)
                .HasConversion<string>()
                .HasColumnName("role")
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(u => u.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired();

            builder.Property(u => u.UpdatedAt)
                .HasColumnName("updated_at");

            builder.Property(u => u.DeletedAt)
                .HasColumnName("deleted_at");

            // Index for email and username for uniqueness
            builder.HasIndex(u => u.Email)
                .IsUnique()
                .HasFilter("\"deleted_at\" IS NULL");

            builder.HasIndex(u => u.Username)
                .IsUnique()
                .HasFilter("\"deleted_at\" IS NULL");
        }
    }
}

