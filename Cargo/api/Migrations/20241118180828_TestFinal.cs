using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class TestFinal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ItemId",
                table: "TransferItemMovement",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ItemIdString",
                table: "TransferItemMovement",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ItemId",
                table: "ShipmentItemMovement",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ItemIdString",
                table: "ShipmentItemMovement",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ItemIdString",
                table: "OrderItemMovement",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ItemIdString",
                table: "TransferItemMovement");

            migrationBuilder.DropColumn(
                name: "ItemIdString",
                table: "ShipmentItemMovement");

            migrationBuilder.DropColumn(
                name: "ItemIdString",
                table: "OrderItemMovement");

            migrationBuilder.AlterColumn<string>(
                name: "ItemId",
                table: "TransferItemMovement",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<string>(
                name: "ItemId",
                table: "ShipmentItemMovement",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");
        }
    }
}
