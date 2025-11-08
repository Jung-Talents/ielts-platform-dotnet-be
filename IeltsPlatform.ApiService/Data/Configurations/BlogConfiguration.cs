using IeltsPlatform.ApiService.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IeltsPlatform.ApiService.Data.Configurations
{
    public class BlogConfiguration : IEntityTypeConfiguration<Blog>
    {
        public void Configure(EntityTypeBuilder<Blog> builder)
        {
            builder.HasKey(b => b.Id);

            builder.Property(b => b.Id)
                        .HasColumnName("id");

            builder.Property(b => b.Name)
                        .HasColumnName("blog_name")
                        .IsRequired()
                        .HasMaxLength(200);

            builder.Property(b => b.Content)
                        .HasColumnName("blog_content")
                        .IsRequired();

            builder.Property(b => b.Status)
                        .HasConversion<string>()
                        .HasColumnName("blog_status")
                        .HasMaxLength(50);

            builder.Property(b => b.Theme)
                        .HasConversion<string>()
                        .HasColumnName("blog_theme")
                        .HasMaxLength(50);

            builder.Property(b => b.UpdatedAt)
                        .HasColumnName("updated_at");

            builder.Property(b => b.CreatedAt)
                        .HasColumnName("created_at")
                        .IsRequired();

            builder.Property(b => b.DeletedAt)
                        .HasColumnName("deleted_at");
        }
    }
}
