using IeltsPlatform.ApiService.Entities;
using IeltsPlatform.ApiService.Enums.IeltsTest;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Threading.Tasks;

namespace IeltsPlatform.ApiService.Data.Configurations
{
    public class IeltsTestConfiguration : IEntityTypeConfiguration<IeltsTest>
    {
        public void Configure(EntityTypeBuilder<IeltsTest> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Id)
                        .HasColumnName("id");

            builder.Property(t => t.Name)
                        .HasColumnName("test_name")
                        .HasMaxLength(200)
                        .IsRequired();

            builder.Property(t => t.Duration)
                        .HasColumnName("duration")
                        .IsRequired();

            builder.Property(t => t.Status)
                        .HasConversion<string>()
                        .HasColumnName("status")
                        .HasMaxLength(50);

            builder.Property(t => t.UpdatedAt)
                        .HasColumnName("updated_at");

            builder.Property(t => t.CreatedAt)
                        .HasColumnName("created_at")
                        .IsRequired();

            builder.Property(t => t.DeletedAt)
                        .HasColumnName("deleted_at");

            // Seed data
            var seedTest = IeltsTest.Create(
                "IELTS Academic Listening Practice Test 1",
                40,
                IeltsTestStatus.Published
            );

            seedTest.Id = Guid.Parse("11111111-1111-1111-1111-111111111111");
            seedTest.CreatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero);
            seedTest.UpdatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero);

            builder.HasData(seedTest);
        }
    }
}