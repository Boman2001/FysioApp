using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Core.Infrastructure.Migrations
{
    public partial class addedSeedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "DeletedAt", "Email", "FirstName", "LastName", "Preposition", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2021, 10, 29, 0, 29, 45, 28, DateTimeKind.Local).AddTicks(5369), null, "Danmaarkaas1@gmail.com", "Dirk", "DoctorMan", "De", null },
                    { 3, new DateTime(2021, 10, 29, 0, 29, 45, 47, DateTimeKind.Local).AddTicks(2966), null, "Danmaarkaas2@gmail.com", "Paula", "PatientenBerg", "van der", null },
                    { 4, new DateTime(2021, 10, 29, 0, 29, 45, 47, DateTimeKind.Local).AddTicks(4474), null, "Danmaarkaas3@gmail.com", "Pavlov", "PatientStan", "", null },
                    { 2, new DateTime(2021, 10, 29, 0, 29, 45, 49, DateTimeKind.Local).AddTicks(9073), null, "Danmaarkaas@gmail.com", "Stefan", "Student", "De", null }
                });

            migrationBuilder.InsertData(
                table: "Patients",
                columns: new[] { "Id", "BirthDay", "City", "Gender", "HouseNumber", "IdNumber", "PatientNumber", "PhoneNumber", "PictureUrl", "PostalCode", "Street" },
                values: new object[,]
                {
                    { 3, new DateTime(1965, 10, 29, 0, 29, 45, 47, DateTimeKind.Local).AddTicks(3627), null, 1, null, null, "dfb87071-7eef-4e81-91ab-461d5e2c5fad", "0636303815", "ee23a151-8ea2-40d6-aad6-9834d3bd4da3_2.jpg", null, null },
                    { 4, new DateTime(2000, 10, 29, 0, 29, 45, 47, DateTimeKind.Local).AddTicks(4487), null, 0, null, null, "5b01effe-2016-4597-94b5-a537db81b354", "0636303816", "506cf9b3-c437-46bd-944e-3dfcb1d17e8b_9.jpg", null, null }
                });

            migrationBuilder.InsertData(
                table: "Staff",
                columns: new[] { "Id", "end", "start" },
                values: new object[,]
                {
                    { 1, new TimeSpan(0, 0, 0, 0, 0), new TimeSpan(0, 0, 0, 0, 0) },
                    { 2, new TimeSpan(0, 0, 0, 0, 0), new TimeSpan(0, 0, 0, 0, 0) }
                });

            migrationBuilder.InsertData(
                table: "Doctors",
                columns: new[] { "Id", "BigNumber", "EmployeeNumber", "PhoneNumber" },
                values: new object[] { 1, "29292929929", "0636303815", "0636303815" });

            migrationBuilder.InsertData(
                table: "Students",
                columns: new[] { "Id", "StudentNumber" },
                values: new object[] { 2, "2153494" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Doctors",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Patients",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Patients",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Staff",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Staff",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
