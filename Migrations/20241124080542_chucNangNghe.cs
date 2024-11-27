using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace elearning_b1.Migrations
{
    /// <inheritdoc />
    public partial class chucNangNghe : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Listenings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AudioUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Transcript = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Listenings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ListeningsQuestions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ListeningId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListeningsQuestions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ListeningsQuestions_Listenings_ListeningId",
                        column: x => x.ListeningId,
                        principalTable: "Listenings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ListeningsOptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsCorrect = table.Column<bool>(type: "bit", nullable: false),
                    ListeningQuestionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListeningsOptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ListeningsOptions_ListeningsQuestions_ListeningQuestionId",
                        column: x => x.ListeningQuestionId,
                        principalTable: "ListeningsQuestions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ListeningsOptions_ListeningQuestionId",
                table: "ListeningsOptions",
                column: "ListeningQuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_ListeningsQuestions_ListeningId",
                table: "ListeningsQuestions",
                column: "ListeningId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ListeningsOptions");

            migrationBuilder.DropTable(
                name: "ListeningsQuestions");

            migrationBuilder.DropTable(
                name: "Listenings");
        }
    }
}
