using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkyGlobal.Data.Migrations
{
    /// <inheritdoc />
    public partial class EventspusWorkUpdates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EventCategories",
                columns: table => new
                {
                    EventCategorieId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventCategorieName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventCategories", x => x.EventCategorieId);
                });

            migrationBuilder.CreateTable(
                name: "WorkUpdateTopics",
                columns: table => new
                {
                    WorkUpdateTopicId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Topic = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Topicpicture = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkUpdateTopics", x => x.WorkUpdateTopicId);
                });

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    EventId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EventDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EventTime = table.Column<TimeOnly>(type: "time", nullable: false),
                    EventDate = table.Column<DateOnly>(type: "date", nullable: false),
                    EventThumbnail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EventCategorieId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.EventId);
                    table.ForeignKey(
                        name: "FK_Events_EventCategories_EventCategorieId",
                        column: x => x.EventCategorieId,
                        principalTable: "EventCategories",
                        principalColumn: "EventCategorieId");
                });

            migrationBuilder.CreateTable(
                name: "WorkUpdates",
                columns: table => new
                {
                    WorkUpdateId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ArticlePicture = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Update = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Author = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NewsGenreId = table.Column<int>(type: "int", nullable: true),
                    WorkUpdateTopicId = table.Column<int>(type: "int", nullable: true),
                    TrendingNow = table.Column<bool>(type: "bit", nullable: true),
                    BreakingNews = table.Column<bool>(type: "bit", nullable: true),
                    IsPublished = table.Column<bool>(type: "bit", nullable: true),
                    PickOfMonth = table.Column<bool>(type: "bit", nullable: true),
                    EditorsPick = table.Column<bool>(type: "bit", nullable: true),
                    DatePublished = table.Column<DateOnly>(type: "date", nullable: true),
                    ReadingDuration = table.Column<int>(type: "int", nullable: true),
                    IsApproved = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkUpdates", x => x.WorkUpdateId);
                    table.ForeignKey(
                        name: "FK_WorkUpdates_WorkUpdateTopics_WorkUpdateTopicId",
                        column: x => x.WorkUpdateTopicId,
                        principalTable: "WorkUpdateTopics",
                        principalColumn: "WorkUpdateTopicId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Events_EventCategorieId",
                table: "Events",
                column: "EventCategorieId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkUpdates_WorkUpdateTopicId",
                table: "WorkUpdates",
                column: "WorkUpdateTopicId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "WorkUpdates");

            migrationBuilder.DropTable(
                name: "EventCategories");

            migrationBuilder.DropTable(
                name: "WorkUpdateTopics");
        }
    }
}
