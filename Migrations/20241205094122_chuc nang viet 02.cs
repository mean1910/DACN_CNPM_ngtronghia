using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace elearning_b1.Migrations
{
    /// <inheritdoc />
    public partial class chucnangviet02 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Submissions_Writings_WritingId",
                table: "Submissions");

            migrationBuilder.DropIndex(
                name: "IX_Submissions_WritingId",
                table: "Submissions");

            migrationBuilder.DropColumn(
                name: "WritingTopicId",
                table: "Submissions");

            migrationBuilder.AlterColumn<string>(
                name: "UserAnswer",
                table: "Submissions",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserAnswer",
                table: "Submissions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WritingTopicId",
                table: "Submissions",
                type: "int",
                nullable: false,
                defaultValue: 0);

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
    }
}
