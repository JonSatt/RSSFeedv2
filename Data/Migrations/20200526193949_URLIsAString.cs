using Microsoft.EntityFrameworkCore.Migrations;

namespace RSSFeedv2.Data.Migrations
{
    public partial class URLIsAString : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "FeedURL",
                table: "Subscription",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "FeedURL",
                table: "Subscription",
                type: "int",
                nullable: false,
                oldClrType: typeof(string));
        }
    }
}
