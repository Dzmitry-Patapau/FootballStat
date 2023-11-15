using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FootballStats.Data.Migrations
{
    /// <inheritdoc />
    public partial class delFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Country",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "Nationality",
                table: "Referees");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "Leagues");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Teams",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Nationality",
                table: "Referees",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Leagues",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
