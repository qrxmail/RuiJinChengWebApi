using Microsoft.EntityFrameworkCore.Migrations;

namespace SingleWellWebApi.Migrations
{
    public partial class eidtFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OilPot",
                table: "OilStation");

            migrationBuilder.AddColumn<string>(
                name: "OilPot",
                table: "WorkTicket",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OilPot",
                table: "WorkTicket");

            migrationBuilder.AddColumn<string>(
                name: "OilPot",
                table: "OilStation",
                type: "text",
                nullable: true);
        }
    }
}
