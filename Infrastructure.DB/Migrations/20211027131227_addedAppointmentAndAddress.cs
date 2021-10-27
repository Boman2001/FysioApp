using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Core.Infrastructure.Migrations
{
    public partial class addedAppointmentAndAddress : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExcecutedOn",
                table: "Treatment");

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

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<DateTime>(
                name: "ExcecutedOn",
                table: "Treatment",
                type: "datetime2",
                nullable: true);
        }
    }
}
