using Microsoft.EntityFrameworkCore.Migrations;

namespace TicketStore.Migrations
{
    public partial class locationY : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Locationy",
                table: "Event",
                newName: "LocationY");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LocationY",
                table: "Event",
                newName: "Locationy");
        }
    }
}
