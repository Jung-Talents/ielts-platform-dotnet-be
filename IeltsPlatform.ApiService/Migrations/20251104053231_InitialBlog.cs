using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IeltsPlatform.ApiService.Migrations
{
    /// <inheritdoc />
    public partial class InitialBlog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Blogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Blog_name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Blog_content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Blog_status = table.Column<int>(type: "int", nullable: false),
                    Blog_theme = table.Column<int>(type: "int", nullable: false),
                    Updated_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    Created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Deleted_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blogs", x => x.Id);
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
