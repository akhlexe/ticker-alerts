using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TickerAlert.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FinancialAssets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ticker = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinancialAssets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Alerts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FinancialAssetId = table.Column<int>(type: "int", nullable: false),
                    TargetPrice = table.Column<decimal>(type: "decimal(15,2)", nullable: false),
                    ThresholdType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Alerts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Alerts_FinancialAssets_FinancialAssetId",
                        column: x => x.FinancialAssetId,
                        principalTable: "FinancialAssets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PriceMeasures",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FinancialAssetId = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(15,2)", nullable: false),
                    MeasuredOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PriceMeasures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PriceMeasures_FinancialAssets_FinancialAssetId",
                        column: x => x.FinancialAssetId,
                        principalTable: "FinancialAssets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Alerts_FinancialAssetId",
                table: "Alerts",
                column: "FinancialAssetId");

            migrationBuilder.CreateIndex(
                name: "IX_PriceMeasures_FinancialAssetId",
                table: "PriceMeasures",
                column: "FinancialAssetId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Alerts");

            migrationBuilder.DropTable(
                name: "PriceMeasures");

            migrationBuilder.DropTable(
                name: "FinancialAssets");
        }
    }
}
