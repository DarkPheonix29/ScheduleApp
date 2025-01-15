using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDatabaseModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InstructorCard",
                table: "UserProfiles");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "UserProfiles",
                newName: "ProfileId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "StudentLessons",
                newName: "LessonId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "InstructorAvailabilities",
                newName: "AvailabilityId");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_UserProfiles_Email",
                table: "UserProfiles",
                column: "Email");

            migrationBuilder.CreateTable(
                name: "ExcelData",
                columns: table => new
                {
                    ExcelDataId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ProfileId = table.Column<int>(type: "INTEGER", nullable: false),
                    Category = table.Column<string>(type: "TEXT", nullable: false),
                    Topic = table.Column<string>(type: "TEXT", nullable: false),
                    Subtopic = table.Column<string>(type: "TEXT", nullable: false),
                    Les1 = table.Column<string>(type: "TEXT", nullable: false),
                    Les2 = table.Column<string>(type: "TEXT", nullable: false),
                    Les3 = table.Column<string>(type: "TEXT", nullable: false),
                    Les4 = table.Column<string>(type: "TEXT", nullable: false),
                    Les5 = table.Column<string>(type: "TEXT", nullable: false),
                    Les6 = table.Column<string>(type: "TEXT", nullable: false),
                    Les7 = table.Column<string>(type: "TEXT", nullable: false),
                    Les8 = table.Column<string>(type: "TEXT", nullable: false),
                    Les9 = table.Column<string>(type: "TEXT", nullable: false),
                    Les10 = table.Column<string>(type: "TEXT", nullable: false),
                    Les11 = table.Column<string>(type: "TEXT", nullable: false),
                    Les12 = table.Column<string>(type: "TEXT", nullable: false),
                    Les13 = table.Column<string>(type: "TEXT", nullable: false),
                    Les14 = table.Column<string>(type: "TEXT", nullable: false),
                    Les15 = table.Column<string>(type: "TEXT", nullable: false),
                    Les16 = table.Column<string>(type: "TEXT", nullable: false),
                    Les17 = table.Column<string>(type: "TEXT", nullable: false),
                    Les18 = table.Column<string>(type: "TEXT", nullable: false),
                    Les19 = table.Column<string>(type: "TEXT", nullable: false),
                    Les20 = table.Column<string>(type: "TEXT", nullable: false),
                    Les21 = table.Column<string>(type: "TEXT", nullable: false),
                    Les22 = table.Column<string>(type: "TEXT", nullable: false),
                    Les23 = table.Column<string>(type: "TEXT", nullable: false),
                    Les24 = table.Column<string>(type: "TEXT", nullable: false),
                    Les25 = table.Column<string>(type: "TEXT", nullable: false),
                    Les26 = table.Column<string>(type: "TEXT", nullable: false),
                    Les27 = table.Column<string>(type: "TEXT", nullable: false),
                    Les28 = table.Column<string>(type: "TEXT", nullable: false),
                    Les29 = table.Column<string>(type: "TEXT", nullable: false),
                    Les30 = table.Column<string>(type: "TEXT", nullable: false),
                    Les31 = table.Column<string>(type: "TEXT", nullable: false),
                    Les32 = table.Column<string>(type: "TEXT", nullable: false),
                    Les33 = table.Column<string>(type: "TEXT", nullable: false),
                    Les34 = table.Column<string>(type: "TEXT", nullable: false),
                    Les35 = table.Column<string>(type: "TEXT", nullable: false),
                    Les36 = table.Column<string>(type: "TEXT", nullable: false),
                    Les37 = table.Column<string>(type: "TEXT", nullable: false),
                    Les38 = table.Column<string>(type: "TEXT", nullable: false),
                    Les39 = table.Column<string>(type: "TEXT", nullable: false),
                    Les40 = table.Column<string>(type: "TEXT", nullable: false),
                    Les41 = table.Column<string>(type: "TEXT", nullable: false),
                    Les42 = table.Column<string>(type: "TEXT", nullable: false),
                    Les43 = table.Column<string>(type: "TEXT", nullable: false),
                    Les44 = table.Column<string>(type: "TEXT", nullable: false),
                    Les45 = table.Column<string>(type: "TEXT", nullable: false),
                    Les46 = table.Column<string>(type: "TEXT", nullable: false),
                    Les47 = table.Column<string>(type: "TEXT", nullable: false),
                    Les48 = table.Column<string>(type: "TEXT", nullable: false),
                    Les49 = table.Column<string>(type: "TEXT", nullable: false),
                    Les50 = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExcelData", x => x.ExcelDataId);
                    table.ForeignKey(
                        name: "FK_ExcelData_UserProfiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "UserProfiles",
                        principalColumn: "ProfileId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_Email",
                table: "UserProfiles",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StudentLessons_InstructorEmail",
                table: "StudentLessons",
                column: "InstructorEmail");

            migrationBuilder.CreateIndex(
                name: "IX_InstructorAvailabilities_InstructorEmail",
                table: "InstructorAvailabilities",
                column: "InstructorEmail");

            migrationBuilder.CreateIndex(
                name: "IX_ExcelData_ProfileId",
                table: "ExcelData",
                column: "ProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_InstructorAvailabilities_UserProfiles_InstructorEmail",
                table: "InstructorAvailabilities",
                column: "InstructorEmail",
                principalTable: "UserProfiles",
                principalColumn: "Email",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentLessons_UserProfiles_InstructorEmail",
                table: "StudentLessons",
                column: "InstructorEmail",
                principalTable: "UserProfiles",
                principalColumn: "Email",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InstructorAvailabilities_UserProfiles_InstructorEmail",
                table: "InstructorAvailabilities");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentLessons_UserProfiles_InstructorEmail",
                table: "StudentLessons");

            migrationBuilder.DropTable(
                name: "ExcelData");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_UserProfiles_Email",
                table: "UserProfiles");

            migrationBuilder.DropIndex(
                name: "IX_UserProfiles_Email",
                table: "UserProfiles");

            migrationBuilder.DropIndex(
                name: "IX_StudentLessons_InstructorEmail",
                table: "StudentLessons");

            migrationBuilder.DropIndex(
                name: "IX_InstructorAvailabilities_InstructorEmail",
                table: "InstructorAvailabilities");

            migrationBuilder.RenameColumn(
                name: "ProfileId",
                table: "UserProfiles",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "LessonId",
                table: "StudentLessons",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "AvailabilityId",
                table: "InstructorAvailabilities",
                newName: "Id");

            migrationBuilder.AddColumn<byte[]>(
                name: "InstructorCard",
                table: "UserProfiles",
                type: "BLOB",
                nullable: true);
        }
    }
}
