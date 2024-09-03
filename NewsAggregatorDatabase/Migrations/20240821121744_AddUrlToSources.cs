using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NewsAggregatorDatabase.Migrations
{
    /// <inheritdoc />
    public partial class AddUrlToSources : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BaseUrl",
                table: "Sources",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RssUrl",
                table: "Sources",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BaseUrl",
                table: "Sources");

            migrationBuilder.DropColumn(
                name: "RssUrl",
                table: "Sources");
        }
    }
}
