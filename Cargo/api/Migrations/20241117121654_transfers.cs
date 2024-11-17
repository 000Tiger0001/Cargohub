using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class transfers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_Transfers_TransferId",
                table: "Items");

            migrationBuilder.DropTable(
                name: "ItemMovement");

            migrationBuilder.DropIndex(
                name: "IX_Items_TransferId",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "TransferId",
                table: "Items");

            migrationBuilder.AlterColumn<int>(
                name: "TransferTo",
                table: "Transfers",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "TransferFrom",
                table: "Transfers",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.CreateTable(
                name: "OrderItemMovement",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    ItemId = table.Column<string>(type: "TEXT", nullable: true),
                    Amount = table.Column<int>(type: "INTEGER", nullable: false),
                    OrderId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItemMovement", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderItemMovement_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ShipmentItemMovement",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    ItemId = table.Column<string>(type: "TEXT", nullable: true),
                    Amount = table.Column<int>(type: "INTEGER", nullable: false),
                    ShipmentId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShipmentItemMovement", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShipmentItemMovement_Shipments_ShipmentId",
                        column: x => x.ShipmentId,
                        principalTable: "Shipments",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TransferItemMovement",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    ItemId = table.Column<string>(type: "TEXT", nullable: true),
                    Amount = table.Column<int>(type: "INTEGER", nullable: false),
                    TransferId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransferItemMovement", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransferItemMovement_Transfers_TransferId",
                        column: x => x.TransferId,
                        principalTable: "Transfers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderItemMovement_OrderId",
                table: "OrderItemMovement",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_ShipmentItemMovement_ShipmentId",
                table: "ShipmentItemMovement",
                column: "ShipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_TransferItemMovement_TransferId",
                table: "TransferItemMovement",
                column: "TransferId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderItemMovement");

            migrationBuilder.DropTable(
                name: "ShipmentItemMovement");

            migrationBuilder.DropTable(
                name: "TransferItemMovement");

            migrationBuilder.AlterColumn<int>(
                name: "TransferTo",
                table: "Transfers",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "TransferFrom",
                table: "Transfers",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TransferId",
                table: "Items",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ItemMovement",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Amount = table.Column<int>(type: "INTEGER", nullable: false),
                    ItemId = table.Column<string>(type: "TEXT", nullable: true),
                    OrderId = table.Column<string>(type: "TEXT", nullable: true),
                    ShipmentId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemMovement", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemMovement_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ItemMovement_Shipments_ShipmentId",
                        column: x => x.ShipmentId,
                        principalTable: "Shipments",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Items_TransferId",
                table: "Items",
                column: "TransferId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemMovement_OrderId",
                table: "ItemMovement",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemMovement_ShipmentId",
                table: "ItemMovement",
                column: "ShipmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Transfers_TransferId",
                table: "Items",
                column: "TransferId",
                principalTable: "Transfers",
                principalColumn: "Id");
        }
    }
}
