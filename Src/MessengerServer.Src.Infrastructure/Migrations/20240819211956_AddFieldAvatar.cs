using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MessengerServer.Src.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddFieldAvatar : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CropAvatar",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FullAvatar",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CropAvatar",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "FullAvatar",
                table: "Users");
        }
    }
}
