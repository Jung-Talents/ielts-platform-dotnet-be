using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IeltsPlatform.ApiService.Migrations
{
    /// <inheritdoc />
    public partial class initialIeltsTest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Blogs",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    blog_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    blog_content = table.Column<string>(type: "text", nullable: false),
                    blog_status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    blog_theme = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    deleted_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blogs", x => x.id);
                });

            //migrationBuilder.CreateTable(
            //    name: "IeltsTests",
            //    columns: table => new
            //    {
            //        id = table.Column<Guid>(type: "uuid", nullable: false),
            //        test_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
            //        duration = table.Column<int>(type: "integer", nullable: false),
            //        status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
            //        updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
            //        created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
            //        deleted_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_IeltsTests", x => x.id);
            //    });

            migrationBuilder.InsertData(
                table: "IeltsTests",
                columns: new[] { "id", "created_at", "deleted_at", "duration", "test_name", "status", "updated_at" },
                values: new object[] { new Guid("11111111-1111-1111-1111-111111111111"), new DateTimeOffset(new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, 40, "IELTS Academic Listening Practice Test 1", "Published", new DateTimeOffset(new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Blogs");

            //migrationBuilder.DropTable(
            //    name: "IeltsTests");
        }
    }
}
