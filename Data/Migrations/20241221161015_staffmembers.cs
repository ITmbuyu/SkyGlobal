using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkyGlobal.Data.Migrations
{
    /// <inheritdoc />
    public partial class staffmembers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StaffMembers",
                columns: table => new
                {
                    StaffMemberId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StaffMemberName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StaffMemberSurname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StaffMemberEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StaffMemberPhone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StaffMemberPassword = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StaffMemberRole = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StaffMemeberRate = table.Column<double>(type: "float", nullable: false),
                    StaffMemeberHours = table.Column<int>(type: "int", nullable: false),
                    TeamId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaffMembers", x => x.StaffMemberId);
                    table.ForeignKey(
                        name: "FK_StaffMembers_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "TeamId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_StaffMembers_TeamId",
                table: "StaffMembers",
                column: "TeamId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StaffMembers");
        }
    }
}
