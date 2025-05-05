using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Facebook.Migrations
{
    /// <inheritdoc />
    public partial class ChangedFriendTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAccepted",
                table: "SendFriendRequests");

            migrationBuilder.DropColumn(
                name: "IsDeclined",
                table: "SendFriendRequests");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "SendFriendRequests",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "SendFriendRequests");

            migrationBuilder.AddColumn<bool>(
                name: "IsAccepted",
                table: "SendFriendRequests",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeclined",
                table: "SendFriendRequests",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }
    }
}
