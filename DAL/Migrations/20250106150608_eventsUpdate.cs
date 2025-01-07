using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class eventsUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InstructorId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "StudentId",
                table: "Events");

            migrationBuilder.AddColumn<string>(
                name: "InstructorEmail",
                table: "Events",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "StudentEmail",
                table: "Events",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InstructorEmail",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "StudentEmail",
                table: "Events");

            migrationBuilder.AddColumn<int>(
                name: "InstructorId",
                table: "Events",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StudentId",
                table: "Events",
                type: "INTEGER",
                nullable: true);
        }
    }
}
