using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace elearning_b1.Migrations
{
    /// <inheritdoc />
    public partial class CapNhatBaiTapTuVung : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Exercises_ExerciseId",
                table: "Questions");

            migrationBuilder.DropTable(
                name: "Exercises");

            migrationBuilder.RenameColumn(
                name: "ExerciseId",
                table: "Questions",
                newName: "TopicId");

            migrationBuilder.RenameIndex(
                name: "IX_Questions_ExerciseId",
                table: "Questions",
                newName: "IX_Questions_TopicId");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Topics_TopicId",
                table: "Questions",
                column: "TopicId",
                principalTable: "Topics",
                principalColumn: "TopicID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Topics_TopicId",
                table: "Questions");

            migrationBuilder.RenameColumn(
                name: "TopicId",
                table: "Questions",
                newName: "ExerciseId");

            migrationBuilder.RenameIndex(
                name: "IX_Questions_TopicId",
                table: "Questions",
                newName: "IX_Questions_ExerciseId");

            migrationBuilder.CreateTable(
                name: "Exercises",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TopicId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exercises", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Exercises_Topics_TopicId",
                        column: x => x.TopicId,
                        principalTable: "Topics",
                        principalColumn: "TopicID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Exercises_TopicId",
                table: "Exercises",
                column: "TopicId");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Exercises_ExerciseId",
                table: "Questions",
                column: "ExerciseId",
                principalTable: "Exercises",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
