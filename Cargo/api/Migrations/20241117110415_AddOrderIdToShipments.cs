using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class AddOrderIdToShipments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Items",
                table: "Shipments");

            migrationBuilder.RenameColumn(
                name: "OrderIds",
                table: "Shipments",
                newName: "OrderId");

            migrationBuilder.AddColumn<string>(
                name: "ShipmentId",
                table: "ItemMovement",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ItemMovement_ShipmentId",
                table: "ItemMovement",
                column: "ShipmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemMovement_Shipments_ShipmentId",
                table: "ItemMovement",
                column: "ShipmentId",
                principalTable: "Shipments",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemMovement_Shipments_ShipmentId",
                table: "ItemMovement");

            migrationBuilder.DropIndex(
                name: "IX_ItemMovement_ShipmentId",
                table: "ItemMovement");

            migrationBuilder.DropColumn(
                name: "ShipmentId",
                table: "ItemMovement");

            migrationBuilder.RenameColumn(
                name: "OrderId",
                table: "Shipments",
                newName: "OrderIds");

            migrationBuilder.AddColumn<string>(
                name: "Items",
                table: "Shipments",
                type: "TEXT",
                nullable: true);
        }
    }
}
