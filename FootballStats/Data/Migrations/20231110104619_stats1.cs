using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FootballStats.Data.Migrations
{
    /// <inheritdoc />
    public partial class stats1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ShotsOffTargetHome",
                table: "MatchStatistics",
                newName: "ShotsAllHome");

            migrationBuilder.RenameColumn(
                name: "ShotsOffTargetAway",
                table: "MatchStatistics",
                newName: "ShotsAllAway");

            migrationBuilder.AlterColumn<int>(
                name: "HomeTeamScore",
                table: "Matches",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "AwayTeamScore",
                table: "Matches",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ShotsAllHome",
                table: "MatchStatistics",
                newName: "ShotsOffTargetHome");

            migrationBuilder.RenameColumn(
                name: "ShotsAllAway",
                table: "MatchStatistics",
                newName: "ShotsOffTargetAway");

            migrationBuilder.AlterColumn<int>(
                name: "HomeTeamScore",
                table: "Matches",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AwayTeamScore",
                table: "Matches",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
