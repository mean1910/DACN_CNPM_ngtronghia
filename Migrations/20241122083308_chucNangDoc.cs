using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace elearning_b1.Migrations
{
    /// <inheritdoc />
    public partial class chucNangDoc : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ReadingSkills",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReadingSkills", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReadingQuestions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReadingSkillId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReadingQuestions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReadingQuestions_ReadingSkills_ReadingSkillId",
                        column: x => x.ReadingSkillId,
                        principalTable: "ReadingSkills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReadingOptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsCorrect = table.Column<bool>(type: "bit", nullable: false),
                    ReadingQuestionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReadingOptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReadingOptions_ReadingQuestions_ReadingQuestionId",
                        column: x => x.ReadingQuestionId,
                        principalTable: "ReadingQuestions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReadingOptions_ReadingQuestionId",
                table: "ReadingOptions",
                column: "ReadingQuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_ReadingQuestions_ReadingSkillId",
                table: "ReadingQuestions",
                column: "ReadingSkillId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReadingOptions");

            migrationBuilder.DropTable(
                name: "ReadingQuestions");

            migrationBuilder.DropTable(
                name: "ReadingSkills");
        }
    }
}
