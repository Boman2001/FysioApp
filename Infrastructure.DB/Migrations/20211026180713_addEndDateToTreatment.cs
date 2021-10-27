using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Core.Infrastructure.Migrations
{
    public partial class addEndDateToTreatment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "TreatmentCodeId",
                table: "Treatment",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<DateTime>(
                name: "TreatmentEndDate",
                table: "Treatment",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DismissionDate",
                table: "Dossiers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TreatmentEndDate",
                table: "Treatment");

            migrationBuilder.DropColumn(
                name: "DismissionDate",
                table: "Dossiers");

            migrationBuilder.AlterColumn<int>(
                name: "TreatmentCodeId",
                table: "Treatment",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
