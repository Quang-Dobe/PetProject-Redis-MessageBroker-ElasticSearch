using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Services.First.Migrations
{
    /// <inheritdoc />
    public partial class Update_Modify_Relationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_Device_DeviceId",
                table: "User");

            migrationBuilder.AlterColumn<Guid>(
                name: "DeviceId",
                table: "User",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_User_Device_DeviceId",
                table: "User",
                column: "DeviceId",
                principalTable: "Device",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_Device_DeviceId",
                table: "User");

            migrationBuilder.AlterColumn<Guid>(
                name: "DeviceId",
                table: "User",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_User_Device_DeviceId",
                table: "User",
                column: "DeviceId",
                principalTable: "Device",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
