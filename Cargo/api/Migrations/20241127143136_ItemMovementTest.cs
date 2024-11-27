using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cargo.Migrations
{
    /// <inheritdoc />
    public partial class ItemMovementTest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItemMovement_Orders_OrderId",
                table: "OrderItemMovement");

            migrationBuilder.DropForeignKey(
                name: "FK_ShipmentItemMovement_Shipments_ShipmentId",
                table: "ShipmentItemMovement");

            migrationBuilder.DropForeignKey(
                name: "FK_TransferItemMovement_Transfers_TransferId",
                table: "TransferItemMovement");

            migrationBuilder.AlterColumn<int>(
                name: "TransferId",
                table: "TransferItemMovement",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ShipmentId",
                table: "ShipmentItemMovement",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "OrderId",
                table: "OrderItemMovement",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItemMovement_Orders_OrderId",
                table: "OrderItemMovement",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ShipmentItemMovement_Shipments_ShipmentId",
                table: "ShipmentItemMovement",
                column: "ShipmentId",
                principalTable: "Shipments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TransferItemMovement_Transfers_TransferId",
                table: "TransferItemMovement",
                column: "TransferId",
                principalTable: "Transfers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItemMovement_Orders_OrderId",
                table: "OrderItemMovement");

            migrationBuilder.DropForeignKey(
                name: "FK_ShipmentItemMovement_Shipments_ShipmentId",
                table: "ShipmentItemMovement");

            migrationBuilder.DropForeignKey(
                name: "FK_TransferItemMovement_Transfers_TransferId",
                table: "TransferItemMovement");

            migrationBuilder.AlterColumn<int>(
                name: "TransferId",
                table: "TransferItemMovement",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "ShipmentId",
                table: "ShipmentItemMovement",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "OrderId",
                table: "OrderItemMovement",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItemMovement_Orders_OrderId",
                table: "OrderItemMovement",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ShipmentItemMovement_Shipments_ShipmentId",
                table: "ShipmentItemMovement",
                column: "ShipmentId",
                principalTable: "Shipments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TransferItemMovement_Transfers_TransferId",
                table: "TransferItemMovement",
                column: "TransferId",
                principalTable: "Transfers",
                principalColumn: "Id");
        }
    }
}
