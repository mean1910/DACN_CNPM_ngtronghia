using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace elearning_b1.Migrations
{
    /// <inheritdoc />
    public partial class capNhaGrammar : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DocFilePath",
                table: "GrammarLessons");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DocFilePath",
                table: "GrammarLessons",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
