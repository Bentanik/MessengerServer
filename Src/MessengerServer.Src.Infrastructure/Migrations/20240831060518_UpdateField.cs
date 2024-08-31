using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MessengerServer.Src.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Student",
                table: "ChatHistories");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Student",
                table: "ChatHistories",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
