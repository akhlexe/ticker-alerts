using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TickerAlert.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCedears : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cedears",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FinancialAssetId = table.Column<Guid>(type: "uuid", nullable: false),
                    Ratio = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cedears", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cedears_FinancialAssets_FinancialAssetId",
                        column: x => x.FinancialAssetId,
                        principalTable: "FinancialAssets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cedears_FinancialAssetId",
                table: "Cedears",
                column: "FinancialAssetId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cedears");
        }
    }
}
