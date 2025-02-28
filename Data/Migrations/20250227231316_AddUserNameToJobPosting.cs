using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkyGlobal.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddUserNameToJobPosting : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "JobPostings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CoverLetter",
                table: "JobApplications",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserName",
                table: "JobPostings");

            migrationBuilder.DropColumn(
                name: "CoverLetter",
                table: "JobApplications");
        }
    }
}
