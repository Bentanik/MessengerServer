using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MessengerServer.Src.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddField_User_CoverPhoto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CropCoverPhoto",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FullCoverPhoto",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CropCoverPhoto",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "FullCoverPhoto",
                table: "Users");
        }
    }
}
