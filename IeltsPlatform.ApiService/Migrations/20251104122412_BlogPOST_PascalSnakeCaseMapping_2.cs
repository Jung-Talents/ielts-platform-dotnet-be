using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IeltsPlatform.ApiService.Migrations
{
    /// <inheritdoc />
    public partial class BlogPOST_PascalSnakeCaseMapping_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "Blogs",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "DeletedAt",
                table: "Blogs",
                newName: "deleted_at");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Blogs",
                newName: "created_at");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "Blogs",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "deleted_at",
                table: "Blogs",
                newName: "DeletedAt");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "Blogs",
                newName: "CreatedAt");
        }
    }
}
