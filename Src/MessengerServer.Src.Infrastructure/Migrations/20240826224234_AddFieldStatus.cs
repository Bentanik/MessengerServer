using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MessengerServer.Src.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddFieldStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Status",
                table: "NotificationAddFriends",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "NotificationAddFriends");
        }
    }
}
