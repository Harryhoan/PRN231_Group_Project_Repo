using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class addAddressReference : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Ensure existing data does not cause conflicts
            migrationBuilder.Sql(@"
                DELETE FROM Orders
                WHERE addressId IN (
                    SELECT addressId
                    FROM Orders
                    GROUP BY addressId
                    HAVING COUNT(*) > 1
                )
            ");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_addressId",
                table: "Orders",
                column: "addressId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Addresses_addressId",
                table: "Orders",
                column: "addressId",
                principalTable: "Addresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction); // Use NoAction to avoid multiple cascade paths
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Addresses_addressId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_addressId",
                table: "Orders");
        }
    }
}
