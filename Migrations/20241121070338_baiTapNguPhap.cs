using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace elearning_b1.Migrations
{
    /// <inheritdoc />
    public partial class baiTapNguPhap : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GrammarExercises",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GrammarId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GrammarExercises", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GrammarExercises_Grammars_GrammarId",
                        column: x => x.GrammarId,
                        principalTable: "Grammars",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GrammarQuestions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GrammarExerciseId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GrammarQuestions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GrammarQuestions_GrammarExercises_GrammarExerciseId",
                        column: x => x.GrammarExerciseId,
                        principalTable: "GrammarExercises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GrammarOptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsCorrect = table.Column<bool>(type: "bit", nullable: false),
                    GrammarQuestionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GrammarOptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GrammarOptions_GrammarQuestions_GrammarQuestionId",
                        column: x => x.GrammarQuestionId,
                        principalTable: "GrammarQuestions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GrammarExercises_GrammarId",
                table: "GrammarExercises",
                column: "GrammarId");

            migrationBuilder.CreateIndex(
                name: "IX_GrammarOptions_GrammarQuestionId",
                table: "GrammarOptions",
                column: "GrammarQuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_GrammarQuestions_GrammarExerciseId",
                table: "GrammarQuestions",
                column: "GrammarExerciseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GrammarOptions");

            migrationBuilder.DropTable(
                name: "GrammarQuestions");

            migrationBuilder.DropTable(
                name: "GrammarExercises");
        }
    }
}
