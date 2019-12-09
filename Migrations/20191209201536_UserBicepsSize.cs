using Microsoft.EntityFrameworkCore.Migrations;

namespace GliwickiDzik.Migrations
{
    public partial class UserBicepsSize : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "BicepsSize",
                table: "UserModel",
                nullable: false,
                defaultValue: 0f);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BicepsSize",
                table: "UserModel");
        }
    }
}
