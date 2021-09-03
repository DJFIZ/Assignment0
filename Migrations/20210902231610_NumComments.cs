using Microsoft.EntityFrameworkCore.Migrations;

namespace Assignment0.Migrations
{
    public partial class NumComments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NumComments",
                table: "Blogs",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumComments",
                table: "Blogs");
        }
    }
}
