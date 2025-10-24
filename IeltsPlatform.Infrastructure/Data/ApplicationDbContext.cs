using IeltsPlatform.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace IeltsPlatform.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    public DbSet<TempUser> TempUsers => Set<TempUser>();
    public DbSet<IeltsTest> IeltsTests => Set<IeltsTest>();
    public DbSet<ListeningSection> ListeningSections => Set<ListeningSection>();
    public DbSet<ReadingSection> ReadingSections => Set<ReadingSection>();
    public DbSet<WritingSection> WritingSections => Set<WritingSection>();
    public DbSet<QuestionGroup> QuestionGroups => Set<QuestionGroup>();
    public DbSet<Question> Questions => Set<Question>();
    public DbSet<AnswerKey> AnswerKeys => Set<AnswerKey>();
    public DbSet<IeltsTestResult> IeltsTestResults => Set<IeltsTestResult>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure snake_case naming convention
        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            entity.SetTableName(ToSnakeCase(entity.GetTableName()!));

            foreach (var property in entity.GetProperties())
            {
                property.SetColumnName(ToSnakeCase(property.GetColumnName()));
            }

            foreach (var key in entity.GetKeys())
            {
                key.SetName(ToSnakeCase(key.GetName()!));
            }

            foreach (var foreignKey in entity.GetForeignKeys())
            {
                foreignKey.SetConstraintName(ToSnakeCase(foreignKey.GetConstraintName()!));
            }

            foreach (var index in entity.GetIndexes())
            {
                index.SetDatabaseName(ToSnakeCase(index.GetDatabaseName()!));
            }
        }

        // User configuration
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("users");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Avatar).HasColumnName("avatar").HasMaxLength(255);
            entity.Property(e => e.Username).HasColumnName("username").IsRequired().HasMaxLength(255);
            entity.Property(e => e.Email).HasColumnName("email").IsRequired().HasMaxLength(320);
            entity.Property(e => e.PhoneNumber).HasColumnName("phone_number").HasMaxLength(20);
            entity.Property(e => e.Password).HasColumnName("password").HasMaxLength(255);
            entity.Property(e => e.IsEmailVerified).HasColumnName("is_email_verified").HasDefaultValue(false);
            entity.Property(e => e.Role).HasColumnName("role").HasConversion<string>().HasDefaultValue(Domain.Enums.UserRole.Student);
            entity.Property(e => e.CreatedAt).HasColumnName("created_at").HasColumnType("timestamp with time zone");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at").HasColumnType("timestamp with time zone");
            entity.Property(e => e.LastLoginAt).HasColumnName("last_login_at").HasColumnType("timestamp with time zone");

            entity.HasIndex(e => e.Email).HasDatabaseName("uq_users_email").IsUnique();
        });

        // RefreshToken configuration
        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.ToTable("refresh_tokens");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Token).HasColumnName("token").IsRequired();
            entity.Property(e => e.ExpiresAt).HasColumnName("expires_at").HasColumnType("timestamp with time zone");
            entity.Property(e => e.Revoked).HasColumnName("revoked").HasDefaultValue(false);
            entity.Property(e => e.IpAddress).HasColumnName("ip_address");
            entity.Property(e => e.UserAgent).HasColumnName("user_agent");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at").HasColumnType("timestamp with time zone");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at").HasColumnType("timestamp with time zone");

            entity.HasOne(e => e.User)
                .WithMany(u => u.RefreshTokens)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(e => e.Token).HasDatabaseName("ix_refresh_tokens_token").IsUnique();
            entity.HasIndex(e => new { e.UserId, e.UserAgent }).HasDatabaseName("ix_refresh_tokens_user_id_user_agent");
        });

        // TempUser configuration
        modelBuilder.Entity<TempUser>(entity =>
        {
            entity.ToTable("temp_users");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Username).HasColumnName("username").IsRequired().HasMaxLength(200);
            entity.Property(e => e.Email).HasColumnName("email").IsRequired().HasMaxLength(254);
            entity.Property(e => e.Password).HasColumnName("password").IsRequired();
            entity.Property(e => e.VerificationCode).HasColumnName("verification_code").IsRequired().HasMaxLength(64);
            entity.Property(e => e.ExpiresAt).HasColumnName("expires_at").HasColumnType("timestamp with time zone");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at").HasColumnType("timestamp with time zone");

            entity.HasIndex(e => e.Email).HasDatabaseName("idx_temp_user_email").IsUnique();
        });

        // IeltsTest configuration
        modelBuilder.Entity<IeltsTest>(entity =>
        {
            entity.ToTable("ielts_tests");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.TestName).HasColumnName("test_name").IsRequired();
            entity.Property(e => e.Skill).HasColumnName("skill").HasConversion<string>().IsRequired();
            entity.Property(e => e.Duration).HasColumnName("duration");
            entity.Property(e => e.Order).HasColumnName("order");
            entity.Property(e => e.Status).HasColumnName("status").HasConversion<string>().HasDefaultValue(Domain.Enums.IeltsTestStatus.Draft);
            entity.Property(e => e.Slug).HasColumnName("slug").IsRequired();
            entity.Property(e => e.CreatedAt).HasColumnName("created_at").HasColumnType("timestamp with time zone");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at").HasColumnType("timestamp with time zone");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at").HasColumnType("timestamp with time zone");

            entity.HasIndex(e => e.TestName).HasDatabaseName("ix_ielts_tests_test_name").IsUnique();
            entity.HasIndex(e => e.Slug).HasDatabaseName("ix_ielts_tests_slug").IsUnique();
            entity.HasIndex(e => new { e.Skill, e.Order }).HasDatabaseName("idx_ielts_tests_skill_order");
            entity.HasIndex(e => e.DeletedAt).HasDatabaseName("idx_ielts_tests_deleted_at");
        });

        // ListeningSection configuration
        modelBuilder.Entity<ListeningSection>(entity =>
        {
            entity.ToTable("listening_sections");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.TestId).HasColumnName("test_id");
            entity.Property(e => e.Title).HasColumnName("title").HasMaxLength(255);
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Order).HasColumnName("order");
            entity.Property(e => e.Audio).HasColumnName("audio").IsRequired();
            entity.Property(e => e.CreatedAt).HasColumnName("created_at").HasColumnType("timestamp with time zone");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at").HasColumnType("timestamp with time zone");

            entity.HasOne(e => e.Test)
                .WithMany()
                .HasForeignKey(e => e.TestId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(e => new { e.TestId, e.Order }).HasDatabaseName("idx_listening_sections_test_order").IsUnique();
        });

        // ReadingSection configuration
        modelBuilder.Entity<ReadingSection>(entity =>
        {
            entity.ToTable("reading_sections");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.TestId).HasColumnName("test_id");
            entity.Property(e => e.Title).HasColumnName("title").HasMaxLength(255);
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Order).HasColumnName("order");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at").HasColumnType("timestamp with time zone");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at").HasColumnType("timestamp with time zone");

            entity.HasOne(e => e.Test)
                .WithMany()
                .HasForeignKey(e => e.TestId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // WritingSection configuration
        modelBuilder.Entity<WritingSection>(entity =>
        {
            entity.ToTable("writing_sections");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.TestId).HasColumnName("test_id");
            entity.Property(e => e.Title).HasColumnName("title").HasMaxLength(255);
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Order).HasColumnName("order");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at").HasColumnType("timestamp with time zone");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at").HasColumnType("timestamp with time zone");

            entity.HasOne(e => e.Test)
                .WithMany()
                .HasForeignKey(e => e.TestId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // QuestionGroup configuration
        modelBuilder.Entity<QuestionGroup>(entity =>
        {
            entity.ToTable("question_groups");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.SectionId).HasColumnName("section_id");
            entity.Property(e => e.SectionType).HasColumnName("section_type").HasConversion<string>();
            entity.Property(e => e.Instruction).HasColumnName("instruction").IsRequired();
            entity.Property(e => e.Type).HasColumnName("type").HasConversion<string>();
            entity.Property(e => e.Order).HasColumnName("order");
            entity.Property(e => e.Category).HasColumnName("category").HasConversion<string>();
            entity.Property(e => e.Image).HasColumnName("image");
            entity.Property(e => e.Content).HasColumnName("content").HasColumnType("jsonb");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at").HasColumnType("timestamp with time zone");

            entity.HasIndex(e => new { e.SectionId, e.SectionType }).HasDatabaseName("idx_question_groups_section_id_section_type");
            entity.HasIndex(e => new { e.SectionId, e.Order }).HasDatabaseName("uq_question_groups_section_order").IsUnique();
        });

        // Question configuration
        modelBuilder.Entity<Question>(entity =>
        {
            entity.ToTable("questions");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.GroupId).HasColumnName("group_id");
            entity.Property(e => e.Title).HasColumnName("title").IsRequired().HasMaxLength(255);
            entity.Property(e => e.Content).HasColumnName("content").HasColumnType("jsonb").IsRequired();
            entity.Property(e => e.Image).HasColumnName("image");
            entity.Property(e => e.Order).HasColumnName("order");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at").HasColumnType("timestamp with time zone");

            entity.HasOne(e => e.Group)
                .WithMany()
                .HasForeignKey(e => e.GroupId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(e => new { e.GroupId, e.Order }).HasDatabaseName("uq_questions_group_order").IsUnique();
        });

        // AnswerKey configuration
        modelBuilder.Entity<AnswerKey>(entity =>
        {
            entity.ToTable("answer_keys");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.QuestionId).HasColumnName("question_id");
            entity.Property(e => e.TestId).HasColumnName("test_id");
            entity.Property(e => e.Answers).HasColumnName("answers").HasColumnType("jsonb").IsRequired();
            entity.Property(e => e.Clarification).HasColumnName("clarification");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at").HasColumnType("timestamp with time zone");

            entity.HasOne(e => e.Question)
                .WithOne()
                .HasForeignKey<AnswerKey>(e => e.QuestionId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Test)
                .WithMany()
                .HasForeignKey(e => e.TestId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(e => e.QuestionId).HasDatabaseName("idx_answer_keys_question_id").IsUnique();
            entity.HasIndex(e => e.TestId).HasDatabaseName("idx_answer_keys_test_id");
        });

        // IeltsTestResult configuration
        modelBuilder.Entity<IeltsTestResult>(entity =>
        {
            entity.ToTable("ielts_test_results");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.TestId).HasColumnName("test_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Score).HasColumnName("score");
            entity.Property(e => e.TotalCorrectAnswers).HasColumnName("total_correct_answers");
            entity.Property(e => e.UserSubmission).HasColumnName("user_submission").HasColumnType("jsonb").IsRequired();
            entity.Property(e => e.DetailAnalysis).HasColumnName("detail_analysis").HasColumnType("jsonb").IsRequired();
            entity.Property(e => e.TimeSpent).HasColumnName("time_spent");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at").HasColumnType("timestamp with time zone");

            entity.HasOne(e => e.Test)
                .WithMany()
                .HasForeignKey(e => e.TestId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(e => new { e.TestId, e.UserId }).HasDatabaseName("idx_ielts_test_result_test_user");
            entity.HasIndex(e => e.TestId).HasDatabaseName("idx_ielts_test_result_test");
            entity.HasIndex(e => e.UserId).HasDatabaseName("idx_ielts_test_result_user");
        });
    }

    private static string ToSnakeCase(string input)
    {
        if (string.IsNullOrEmpty(input)) return input;

        var result = new System.Text.StringBuilder();
        result.Append(char.ToLowerInvariant(input[0]));

        for (int i = 1; i < input.Length; i++)
        {
            if (char.IsUpper(input[i]))
            {
                result.Append('_');
                result.Append(char.ToLowerInvariant(input[i]));
            }
            else
            {
                result.Append(input[i]);
            }
        }

        return result.ToString();
    }

    public override int SaveChanges()
    {
        UpdateTimestamps();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateTimestamps();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void UpdateTimestamps()
    {
        var entries = ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

        foreach (var entry in entries)
        {
            if (entry.Entity.GetType().GetProperty("UpdatedAt") != null && entry.State == EntityState.Modified)
            {
                entry.Property("UpdatedAt").CurrentValue = DateTime.UtcNow;
            }

            if (entry.Entity.GetType().GetProperty("CreatedAt") != null && entry.State == EntityState.Added)
            {
                entry.Property("CreatedAt").CurrentValue = DateTime.UtcNow;
            }
        }
    }
}
