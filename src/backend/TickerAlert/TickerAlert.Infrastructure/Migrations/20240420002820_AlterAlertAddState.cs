using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TickerAlert.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AlterAlertAddState : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "State",
                table: "Alerts",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "State",
                table: "Alerts");
        }
    }
}
