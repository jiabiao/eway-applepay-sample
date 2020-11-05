// Copyright (c) eWAY and Contributors. All rights reserved.
// Licensed under the MIT License

using Microsoft.EntityFrameworkCore.Migrations;

namespace MonkeyStore.Data.Catalog.Migrations
{
    public partial class RenameTransactionFieldApplePayToken2Token : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ApplePayToken",
                table: "Transactions",
                newName: "Token");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Token",
                table: "Transactions",
                newName: "ApplePayToken");
        }
    }
}
