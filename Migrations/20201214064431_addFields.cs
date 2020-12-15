using Microsoft.EntityFrameworkCore.Migrations;

namespace SingleWellWebApi.Migrations
{
    public partial class addFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DriverPhone",
                table: "WorkTicket",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OilPot",
                table: "OilStation",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DriverPhone",
                table: "WorkTicket");

            migrationBuilder.DropColumn(
                name: "OilPot",
                table: "OilStation");
        }
    }
}
