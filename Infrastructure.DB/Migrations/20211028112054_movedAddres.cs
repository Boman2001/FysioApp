using Microsoft.EntityFrameworkCore.Migrations;

namespace Core.Infrastructure.Migrations
{
    public partial class movedAddres : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                table: "Dossiers");

            migrationBuilder.DropColumn(
                name: "Housenumber",
                table: "Dossiers");

            migrationBuilder.DropColumn(
                name: "PostalCode",
                table: "Dossiers");

            migrationBuilder.DropColumn(
                name: "Street",
                table: "Dossiers");

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Patients",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Housenumber",
                table: "Patients",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PostalCode",
                table: "Patients",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Street",
                table: "Patients",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "Housenumber",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "PostalCode",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "Street",
                table: "Patients");

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Dossiers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Housenumber",
                table: "Dossiers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PostalCode",
                table: "Dossiers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Street",
                table: "Dossiers",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
