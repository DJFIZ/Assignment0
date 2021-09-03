using Microsoft.EntityFrameworkCore.Migrations;

namespace Assignment0.Migrations
{
    public partial class BlogIDConsistency : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Blogs",
                newName: "BlogId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BlogId",
                table: "Blogs",
                newName: "Id");
        }
    }
}
