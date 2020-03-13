using Microsoft.EntityFrameworkCore.Migrations;

namespace WebAPI_Identity.Migrations
{
    public partial class issue2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "id",
                table: "Issue",
                newName: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Issue",
                newName: "id");
        }
    }
}
