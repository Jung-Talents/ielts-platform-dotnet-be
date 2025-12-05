using IeltsPlatform.ApiService.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IeltsPlatform.ApiService.Data.Configurations
{
    public class IeltsTestConfiguration : IEntityTypeConfiguration<IeltsTest>
    {
        public void Configure(EntityTypeBuilder<IeltsTest> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Id)
                .HasColumnName("id");

            builder.Property(t => t.TestName)
                .HasColumnName("test_name")
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(t => t.Duration)
                .HasColumnName("duration")
                .IsRequired();

            builder.Property(t => t.Status)
                .HasConversion<string>()
                .HasColumnName("status")
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(t => t.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired();

            builder.Property(t => t.UpdatedAt)
                .HasColumnName("updated_at");

            builder.Property(t => t.DeletedAt)
                .HasColumnName("deleted_at");

            // Relationship with Section
            builder.HasMany(t => t.Sections)
                .WithOne(s => s.IeltsTest)
                .HasForeignKey(s => s.IeltsTestId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

