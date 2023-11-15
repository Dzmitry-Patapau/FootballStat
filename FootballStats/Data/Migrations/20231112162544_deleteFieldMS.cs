using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FootballStats.Data.Migrations
{
    /// <inheritdoc />
    public partial class deleteFieldMS : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FoulsAway",
                table: "MatchStatistics");

            migrationBuilder.DropColumn(
                name: "FoulsHome",
                table: "MatchStatistics");

            migrationBuilder.DropColumn(
                name: "OffsidesAway",
                table: "MatchStatistics");

            migrationBuilder.DropColumn(
                name: "OffsidesHome",
                table: "MatchStatistics");

            migrationBuilder.DropColumn(
                name: "PossessionAway",
                table: "MatchStatistics");

            migrationBuilder.DropColumn(
                name: "PossessionHome",
                table: "MatchStatistics");

            migrationBuilder.DropColumn(
                name: "ShotsAllAway",
                table: "MatchStatistics");

            migrationBuilder.DropColumn(
                name: "ShotsAllHome",
                table: "MatchStatistics");

            migrationBuilder.DropColumn(
                name: "ShotsOnTargetAway",
                table: "MatchStatistics");

            migrationBuilder.DropColumn(
                name: "ShotsOnTargetHome",
                table: "MatchStatistics");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FoulsAway",
                table: "MatchStatistics",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FoulsHome",
                table: "MatchStatistics",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OffsidesAway",
                table: "MatchStatistics",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OffsidesHome",
                table: "MatchStatistics",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PossessionAway",
                table: "MatchStatistics",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PossessionHome",
                table: "MatchStatistics",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ShotsAllAway",
                table: "MatchStatistics",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ShotsAllHome",
                table: "MatchStatistics",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ShotsOnTargetAway",
                table: "MatchStatistics",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ShotsOnTargetHome",
                table: "MatchStatistics",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
