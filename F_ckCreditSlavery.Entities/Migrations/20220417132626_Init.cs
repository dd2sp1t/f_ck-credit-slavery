using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace F_ckCreditSlavery.Entities.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CreditAccount",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    InitialBalance = table.Column<double>(type: "float", nullable: false),
                    CurrentDebtBalance = table.Column<double>(type: "float", nullable: false),
                    MonthlyPaymentValue = table.Column<double>(type: "float", nullable: false),
                    MonthlyPaymentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreditAccount", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CreditAccountChange",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreditAccountId = table.Column<int>(type: "int", nullable: false),
                    PaymentValue = table.Column<double>(type: "float", nullable: false),
                    PaymentType = table.Column<int>(type: "int", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreditAccountChange", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CreditAccountChange_CreditAccount_CreditAccountId",
                        column: x => x.CreditAccountId,
                        principalTable: "CreditAccount",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CreditAccountChange_CreditAccountId",
                table: "CreditAccountChange",
                column: "CreditAccountId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CreditAccountChange");

            migrationBuilder.DropTable(
                name: "CreditAccount");
        }
    }
}
