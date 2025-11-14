using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace IeltsPlatform.ApiService.Migrations
{
    /// <inheritdoc />
    public partial class SeedBlogData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Blogs",
                columns: new[] { "Id", "BlogContent", "BlogName", "CreatedAt", "DeletedAt", "Status", "Theme", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), "Master IELTS Reading with these essential tips:\n1. Time management is crucial\n2. Skim and scan effectively\n3. Practice different question types\n4. Build your vocabulary\n5. Read academic articles regularly", "IELTS Reading Tips and Strategies", new DateTime(2025, 11, 5, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, new DateTime(2025, 11, 5, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("17c59ee6-33fa-4310-8480-772a047f18f6"), "Perfect your IELTS Writing Task 2 structure:\n1. Introduction\n   - Background statement\n   - Thesis statement\n2. Body Paragraphs\n   - Topic sentences\n   - Supporting points\n   - Examples\n3. Conclusion\n   - Restate main points\n   - Final thoughts", "IELTS Writing Task 2: Essay Structure", new DateTime(2025, 11, 5, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 3, new DateTime(2025, 11, 5, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("7238e7b3-52fc-4354-ab67-2940cacc2ae5"), "Enhance your listening skills with these note-taking strategies:\n1. Use abbreviations\n2. Focus on keywords\n3. Practice prediction\n4. Listen for signpost words\n5. Review and transfer answers carefully", "IELTS Listening: Note-Taking Techniques", new DateTime(2025, 11, 5, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 0, new DateTime(2025, 11, 5, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("91f5ca49-8983-4390-937d-ecd4907082e2"), "Improve your IELTS Speaking Part 2 performance:\n1. Structure your response (past, present, future)\n2. Include personal experiences\n3. Use advanced vocabulary\n4. Practice time management (2 minutes)\n5. Record yourself speaking", "How to Excel in IELTS Speaking Part 2", new DateTime(2025, 11, 5, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 2, new DateTime(2025, 11, 5, 0, 0, 0, 0, DateTimeKind.Utc) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Blogs",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"));

            migrationBuilder.DeleteData(
                table: "Blogs",
                keyColumn: "Id",
                keyValue: new Guid("17c59ee6-33fa-4310-8480-772a047f18f6"));

            migrationBuilder.DeleteData(
                table: "Blogs",
                keyColumn: "Id",
                keyValue: new Guid("7238e7b3-52fc-4354-ab67-2940cacc2ae5"));

            migrationBuilder.DeleteData(
                table: "Blogs",
                keyColumn: "Id",
                keyValue: new Guid("91f5ca49-8983-4390-937d-ecd4907082e2"));
        }
    }
}
