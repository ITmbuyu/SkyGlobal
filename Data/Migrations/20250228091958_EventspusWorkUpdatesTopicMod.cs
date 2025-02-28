using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkyGlobal.Data.Migrations
{
    /// <inheritdoc />
    public partial class EventspusWorkUpdatesTopicMod : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NewsGenreId",
                table: "WorkUpdates");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NewsGenreId",
                table: "WorkUpdates",
                type: "int",
                nullable: true);
        }
    }
}
