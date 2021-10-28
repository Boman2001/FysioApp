using Microsoft.EntityFrameworkCore.Migrations;

namespace Core.Infrastructure.Migrations
{
    public partial class movedAddres1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Housenumber",
                table: "Patients",
                newName: "HouseNumber");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "HouseNumber",
                table: "Patients",
                newName: "Housenumber");
        }
    }
}
