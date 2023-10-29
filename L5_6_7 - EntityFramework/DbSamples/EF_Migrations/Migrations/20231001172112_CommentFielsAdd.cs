using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _8_EFDemo_Migrations.Migrations
{
    /// <inheritdoc />
    public partial class CommentFielsAdd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "StudentGroupSet",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Comment",
                table: "StudentGroupSet");
        }
    }
}
