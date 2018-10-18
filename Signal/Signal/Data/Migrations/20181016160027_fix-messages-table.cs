using Microsoft.EntityFrameworkCore.Migrations;

namespace Signal.Data.Migrations
{
    public partial class fixmessagestable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Message",
                table: "Contact");

            migrationBuilder.AddColumn<string>(
                name: "Message",
                table: "Messages",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Message",
                table: "Messages");

            migrationBuilder.AddColumn<string>(
                name: "Message",
                table: "Contact",
                nullable: false,
                defaultValue: "");
        }
    }
}
