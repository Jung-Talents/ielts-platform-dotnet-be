using IeltsPlatform.ApiService.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IeltsPlatform.ApiService.Data.Configurations
{
    public class SectionConfiguration : IEntityTypeConfiguration<Section>
    {
        public void Configure(EntityTypeBuilder<Section> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.Id)
                .HasColumnName("id");

            builder.Property(s => s.SectionNumber)
                .HasColumnName("section_number")
                .IsRequired();

            builder.Property(s => s.AudioLink)
                .HasColumnName("audio_link")
                .IsRequired();

            builder.Property(s => s.Transcript)
                .HasColumnName("transcript")
                .IsRequired();

            builder.Property(s => s.IeltsTestId)
                .HasColumnName("ielts_test_id")
                .IsRequired();

            builder.Property(s => s.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired();

            builder.Property(s => s.UpdatedAt)
                .HasColumnName("updated_at");

            builder.Property(s => s.DeletedAt)
                .HasColumnName("deleted_at");

            // Relationship with IeltsTest
            builder.HasOne(s => s.IeltsTest)
                .WithMany(t => t.Sections)
                .HasForeignKey(s => s.IeltsTestId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

