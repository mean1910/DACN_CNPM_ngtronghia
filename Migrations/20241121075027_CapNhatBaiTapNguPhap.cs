using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace elearning_b1.Migrations
{
    /// <inheritdoc />
    public partial class CapNhatBaiTapNguPhap : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GrammarQuestions_GrammarExercises_GrammarExerciseId",
                table: "GrammarQuestions");

            migrationBuilder.DropTable(
                name: "GrammarExercises");

            migrationBuilder.DropTable(
                name: "GrammarLessons");

            migrationBuilder.RenameColumn(
                name: "GrammarExerciseId",
                table: "GrammarQuestions",
                newName: "GrammarId");

            migrationBuilder.RenameIndex(
                name: "IX_GrammarQuestions_GrammarExerciseId",
                table: "GrammarQuestions",
                newName: "IX_GrammarQuestions_GrammarId");

            migrationBuilder.AddForeignKey(
                name: "FK_GrammarQuestions_Grammars_GrammarId",
                table: "GrammarQuestions",
                column: "GrammarId",
                principalTable: "Grammars",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GrammarQuestions_Grammars_GrammarId",
                table: "GrammarQuestions");

            migrationBuilder.RenameColumn(
                name: "GrammarId",
                table: "GrammarQuestions",
                newName: "GrammarExerciseId");

            migrationBuilder.RenameIndex(
                name: "IX_GrammarQuestions_GrammarId",
                table: "GrammarQuestions",
                newName: "IX_GrammarQuestions_GrammarExerciseId");

            migrationBuilder.CreateTable(
                name: "GrammarExercises",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GrammarId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false)
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
                name: "GrammarLessons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DocFilePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PdfFilePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GrammarLessons", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GrammarExercises_GrammarId",
                table: "GrammarExercises",
                column: "GrammarId");

            migrationBuilder.AddForeignKey(
                name: "FK_GrammarQuestions_GrammarExercises_GrammarExerciseId",
                table: "GrammarQuestions",
                column: "GrammarExerciseId",
                principalTable: "GrammarExercises",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
