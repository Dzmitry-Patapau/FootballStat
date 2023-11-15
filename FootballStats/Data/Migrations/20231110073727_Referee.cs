using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FootballStats.Data.Migrations
{
    /// <inheritdoc />
    public partial class Referee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "Referees",
                newName: "Nationality");

            migrationBuilder.RenameColumn(
                name: "FirstName",
                table: "Referees",
                newName: "Name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Nationality",
                table: "Referees",
                newName: "LastName");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Referees",
                newName: "FirstName");
        }
    }
}
