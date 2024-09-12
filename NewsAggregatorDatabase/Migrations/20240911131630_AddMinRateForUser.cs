using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NewsAggregatorDatabase.Migrations
{
    /// <inheritdoc />
    public partial class AddMinRateForUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MinRate",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MinRate",
                table: "Users");
        }
    }
}
