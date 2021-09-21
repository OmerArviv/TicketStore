using Microsoft.EntityFrameworkCore.Migrations;

namespace TicketStore.Migrations
{
    public partial class check1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ahava",
                table: "User");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ahava",
                table: "User",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
