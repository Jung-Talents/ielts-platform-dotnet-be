using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IeltsPlatform.ApiService.Migrations
{
    /// <inheritdoc />
    public partial class BlogPOST_PascalSnakeCaseMapping : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Blog_theme",
                table: "Blogs",
                newName: "blog_theme");

            migrationBuilder.RenameColumn(
                name: "Blog_status",
                table: "Blogs",
                newName: "blog_status");

            migrationBuilder.RenameColumn(
                name: "Blog_name",
                table: "Blogs",
                newName: "blog_name");

            migrationBuilder.RenameColumn(
                name: "Blog_content",
                table: "Blogs",
                newName: "blog_content");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Blogs",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Updated_at",
                table: "Blogs",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "Deleted_at",
                table: "Blogs",
                newName: "DeletedAt");

            migrationBuilder.RenameColumn(
                name: "Created_at",
                table: "Blogs",
                newName: "CreatedAt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "blog_theme",
                table: "Blogs",
                newName: "Blog_theme");

            migrationBuilder.RenameColumn(
                name: "blog_status",
                table: "Blogs",
                newName: "Blog_status");

            migrationBuilder.RenameColumn(
                name: "blog_name",
                table: "Blogs",
                newName: "Blog_name");

            migrationBuilder.RenameColumn(
                name: "blog_content",
                table: "Blogs",
                newName: "Blog_content");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Blogs",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "Blogs",
                newName: "Updated_at");

            migrationBuilder.RenameColumn(
                name: "DeletedAt",
                table: "Blogs",
                newName: "Deleted_at");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Blogs",
                newName: "Created_at");
        }
    }
}
