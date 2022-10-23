using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TescEvents.Migrations
{
    public partial class AddEventsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder) {
            migrationBuilder.CreateTable(
                                         name: "Events",
                                         columns: table => new {
                                             Id = table.Column<Guid>(type: "uuid", nullable: false),
                                             Title = table.Column<string>(type: "character varying(255)",
                                                                          maxLength: 255, nullable: false),
                                             Description = table.Column<string>(type: "text", nullable: false),
                                             Thumbnail = table.Column<string>(type: "text", nullable: true),
                                             Cover = table.Column<string>(type: "text", nullable: true),
                                             Start = table.Column<DateTime>(type: "timestamp with time zone",
                                                                            nullable: false),
                                             End = table.Column<DateTime>(type: "timestamp with time zone",
                                                                          nullable: false),
                                             Archived = table.Column<bool>(type: "boolean", nullable: false)
                                         },
                                         constraints: table => { table.PrimaryKey("PK_Events", x => x.Id); });
        }

        protected override void Down(MigrationBuilder migrationBuilder) {
            migrationBuilder.DropTable(
                                       name: "Events");
        }
    }
}
