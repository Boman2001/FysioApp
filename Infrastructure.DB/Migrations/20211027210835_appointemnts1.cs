using Microsoft.EntityFrameworkCore.Migrations;

namespace Core.Infrastructure.Migrations
{
    public partial class appointemnts1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Treatment_Dossiers_DossierId",
                table: "Treatment");

            migrationBuilder.DropForeignKey(
                name: "FK_Treatment_Staff_ExcecutedById",
                table: "Treatment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Treatment",
                table: "Treatment");

            migrationBuilder.RenameTable(
                name: "Treatment",
                newName: "Appointments");

            migrationBuilder.RenameIndex(
                name: "IX_Treatment_ExcecutedById",
                table: "Appointments",
                newName: "IX_Appointments_ExcecutedById");

            migrationBuilder.RenameIndex(
                name: "IX_Treatment_DossierId",
                table: "Appointments",
                newName: "IX_Appointments_DossierId");

            migrationBuilder.AddColumn<int>(
                name: "DossierId1",
                table: "Appointments",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "isTreatment",
                table: "Appointments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Appointments",
                table: "Appointments",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_DossierId1",
                table: "Appointments",
                column: "DossierId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Dossiers_DossierId",
                table: "Appointments",
                column: "DossierId",
                principalTable: "Dossiers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Dossiers_DossierId1",
                table: "Appointments",
                column: "DossierId1",
                principalTable: "Dossiers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Staff_ExcecutedById",
                table: "Appointments",
                column: "ExcecutedById",
                principalTable: "Staff",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Dossiers_DossierId",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Dossiers_DossierId1",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Staff_ExcecutedById",
                table: "Appointments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Appointments",
                table: "Appointments");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_DossierId1",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "DossierId1",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "isTreatment",
                table: "Appointments");

            migrationBuilder.RenameTable(
                name: "Appointments",
                newName: "Treatment");

            migrationBuilder.RenameIndex(
                name: "IX_Appointments_ExcecutedById",
                table: "Treatment",
                newName: "IX_Treatment_ExcecutedById");

            migrationBuilder.RenameIndex(
                name: "IX_Appointments_DossierId",
                table: "Treatment",
                newName: "IX_Treatment_DossierId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Treatment",
                table: "Treatment",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Treatment_Dossiers_DossierId",
                table: "Treatment",
                column: "DossierId",
                principalTable: "Dossiers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Treatment_Staff_ExcecutedById",
                table: "Treatment",
                column: "ExcecutedById",
                principalTable: "Staff",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
