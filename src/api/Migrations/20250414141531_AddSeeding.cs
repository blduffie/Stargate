using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StargateAPI.Migrations
{
    public partial class AddSeeding : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1) Seed Person rows
            migrationBuilder.InsertData(
                table: "Person",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "John Doe" },
                    { 2, "Jane Doe" },
                    { 3, "Cool Guy" },
                    { 4, "Not so cool guy" }
                }
            );

            // 2) Seed Astronaut rows
            // NOTE: TPT => This references the same Id as Person rows above.
            // If you want to treat Person #1 and #2 as Astronauts, you can do:
            migrationBuilder.InsertData(
                table: "Astronaut",
                columns: new[] { "Id", "Rank", "CurrentDutyTitle", "CareerStartDate", "CareerEndDate" },
                values: new object[,]
                {
                    { 1, "Commander", "Pilot", DateTime.UtcNow, null },
                    { 2, "Lieutenant", "Engineer", new DateTime(2025, 1, 1), null }
                }
            );

            // 3) Seed AstronautDuty rows
            // Here, "AstronautId" must match an Astronaut row you just inserted
            migrationBuilder.InsertData(
                table: "AstronautDuty",
                columns: new[] { "Id", "AstronautId", "DutyTitle", "Rank", "DutyStartDate", "DutyEndDate" },
                values: new object[,]
                {
                    { 1, 1, "Lunar Mission", "Commander", DateTime.UtcNow, null },
                    { 2, 1, "Research Shuttle", "Commander", new DateTime(2025, 2, 1), new DateTime(2025, 2, 10) },
                    { 3, 2, "Space Station Maintenance", "Lieutenant", new DateTime(2025, 1, 10), null }
                }
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Delete the inserted rows in reverse dependency order:

            migrationBuilder.DeleteData(
                table: "AstronautDuty",
                keyColumn: "Id",
                keyValue: 3);
            migrationBuilder.DeleteData(
                table: "AstronautDuty",
                keyColumn: "Id",
                keyValue: 2);
            migrationBuilder.DeleteData(
                table: "AstronautDuty",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Astronaut",
                keyColumn: "Id",
                keyValue: 2);
            migrationBuilder.DeleteData(
                table: "Astronaut",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Person",
                keyColumn: "Id",
                keyValue: 4);
            migrationBuilder.DeleteData(
                table: "Person",
                keyColumn: "Id",
                keyValue: 3);
            migrationBuilder.DeleteData(
                table: "Person",
                keyColumn: "Id",
                keyValue: 2);
            migrationBuilder.DeleteData(
                table: "Person",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
