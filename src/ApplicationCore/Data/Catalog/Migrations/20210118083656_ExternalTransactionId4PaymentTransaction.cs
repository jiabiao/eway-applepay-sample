using Microsoft.EntityFrameworkCore.Migrations;

namespace MonkeyStore.Data.Catalog.Migrations
{
    public partial class ExternalTransactionId4PaymentTransaction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ExternalTransactionId",
                table: "Transactions",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GatewayType",
                table: "Transactions",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExternalTransactionId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "GatewayType",
                table: "Transactions");
        }
    }
}
