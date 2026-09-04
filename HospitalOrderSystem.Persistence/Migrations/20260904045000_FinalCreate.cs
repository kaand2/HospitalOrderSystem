using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitalOrderSystem.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class FinalCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CurrentOrderId",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_CurrentOrderId",
                table: "Users",
                column: "CurrentOrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Orders_CurrentOrderId",
                table: "Users",
                column: "CurrentOrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Orders_CurrentOrderId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_CurrentOrderId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CurrentOrderId",
                table: "Users");
        }
    }
}
