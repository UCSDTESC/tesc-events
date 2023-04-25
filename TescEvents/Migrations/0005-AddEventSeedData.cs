using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TescEvents.Migrations
{
    public partial class _0005AddEventSeedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Events",
                columns: new[] { "Id", "AcceptingApplications", "Archived", "CloseDate", "Cover", "Description", "End", "OpenDate", "RequiresApplication", "RequiresResume", "Start", "Thumbnail", "Title" },
                values: new object[] { new Guid("48f28d9c-b42f-4fe9-923a-eda8fc83aea2"), false, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Come out to our New Event!", new DateTime(2020, 2, 21, 7, 20, 20, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, false, new DateTime(2020, 2, 21, 4, 20, 20, 0, DateTimeKind.Utc), null, "A New Event" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "Id",
                keyValue: new Guid("48f28d9c-b42f-4fe9-923a-eda8fc83aea2"));
        }
    }
}
