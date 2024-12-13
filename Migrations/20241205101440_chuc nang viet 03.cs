using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace elearning_b1.Migrations
{
    /// <inheritdoc />
    public partial class chucnangviet03 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Submissions_WritingId",
                table: "Submissions",
                column: "WritingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Submissions_Writings_WritingId",
                table: "Submissions",
                column: "WritingId",
                principalTable: "Writings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Submissions_Writings_WritingId",
                table: "Submissions");

            migrationBuilder.DropIndex(
                name: "IX_Submissions_WritingId",
                table: "Submissions");
        }
    }
}
