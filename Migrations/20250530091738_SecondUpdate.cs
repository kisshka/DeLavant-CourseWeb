using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DeLavant_CourseWeb.Migrations
{
    /// <inheritdoc />
    public partial class SecondUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Areas_IdArea",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<int>(
                name: "IdArea",
                table: "AspNetUsers",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Areas_IdArea",
                table: "AspNetUsers",
                column: "IdArea",
                principalTable: "Areas",
                principalColumn: "IdArea");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Areas_IdArea",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<int>(
                name: "IdArea",
                table: "AspNetUsers",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Areas_IdArea",
                table: "AspNetUsers",
                column: "IdArea",
                principalTable: "Areas",
                principalColumn: "IdArea",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
