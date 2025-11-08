using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace IeltsPlatform.ApiService.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Blogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    BlogName = table.Column<string>(type: "text", nullable: false),
                    BlogContent = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Theme = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blogs", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Blogs",
                columns: new[] { "Id", "BlogContent", "BlogName", "CreatedAt", "DeletedAt", "Status", "Theme", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), "Master IELTS Reading with these essential tips:\r\n                         1. Time management is crucial\r\n                         2. Skim and scan effectively\r\n                         3. Practice different question types\r\n                         4. Build your vocabulary\r\n                         5. Read academic articles regularly", "IELTS Reading Tips and Strategies", new DateTime(2025, 11, 5, 0, 0, 0, 0, DateTimeKind.Utc), null, 0, 0, new DateTime(2025, 11, 5, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("7213d0f3-e858-41b0-aa4f-1d137218249b"), "Enhance your listening skills with these note-taking strategies:\r\n                         1. Use abbreviations\r\n                         2. Focus on keywords\r\n                         3. Practice prediction\r\n                         4. Listen for signpost words\r\n                         5. Review and transfer answers carefully", "IELTS Listening: Note-Taking Techniques", new DateTime(2025, 11, 5, 0, 0, 0, 0, DateTimeKind.Utc), null, 0, 3, new DateTime(2025, 11, 5, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("7494c2de-1f38-4a75-8580-8be0a4eee505"), "Improve your IELTS Speaking Part 2 performance:\r\n                         1. Structure your response (past, present, future)\r\n                         2. Include personal experiences\r\n                         3. Use advanced vocabulary\r\n                         4. Practice time management (2 minutes)\r\n                         5. Record yourself speaking", "How to Excel in IELTS Speaking Part 2", new DateTime(2025, 11, 5, 0, 0, 0, 0, DateTimeKind.Utc), null, 0, 2, new DateTime(2025, 11, 5, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("f85fe85d-46f3-41bb-8dbb-49cc5f09b55a"), "Perfect your IELTS Writing Task 2 structure:\r\n                         1. Introduction\r\n                            - Background statement\r\n                            - Thesis statement\r\n                         2. Body Paragraphs\r\n                            - Topic sentences\r\n                            - Supporting points\r\n                            - Examples\r\n                         3. Conclusion\r\n                            - Restate main points\r\n                            - Final thoughts", "IELTS Writing Task 2: Essay Structure", new DateTime(2025, 11, 5, 0, 0, 0, 0, DateTimeKind.Utc), null, 0, 1, new DateTime(2025, 11, 5, 0, 0, 0, 0, DateTimeKind.Utc) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Blogs");
        }
    }
}
