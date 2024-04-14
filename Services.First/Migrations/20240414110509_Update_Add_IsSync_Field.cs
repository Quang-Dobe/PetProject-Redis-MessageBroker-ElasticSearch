using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Services.First.Migrations
{
    /// <inheritdoc />
    public partial class Update_Add_IsSync_Field : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSync",
                table: "User",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSync",
                table: "Device",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSync",
                table: "User");

            migrationBuilder.DropColumn(
                name: "IsSync",
                table: "Device");
        }
    }
}
