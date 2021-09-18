using Microsoft.EntityFrameworkCore.Migrations;

namespace TicketStore.Migrations
{
    public partial class gender1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Genre2",
                table: "Event",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Genre2",
                table: "Event");
        }
    }
}
