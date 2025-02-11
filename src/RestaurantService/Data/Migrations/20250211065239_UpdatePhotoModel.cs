using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace RestaurantService.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePhotoModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FoodItems_Photo_PhotoId",
                table: "FoodItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Restaurants_Photo_BannerId",
                table: "Restaurants");

            migrationBuilder.DropForeignKey(
                name: "FK_Restaurants_Photo_LogoId",
                table: "Restaurants");

            migrationBuilder.DropTable(
                name: "Photo");

            migrationBuilder.DropIndex(
                name: "IX_Restaurants_BannerId",
                table: "Restaurants");

            migrationBuilder.DropIndex(
                name: "IX_Restaurants_LogoId",
                table: "Restaurants");

            migrationBuilder.DropIndex(
                name: "IX_FoodItems_PhotoId",
                table: "FoodItems");

            migrationBuilder.DropColumn(
                name: "BannerId",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "LogoId",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "PhotoId",
                table: "FoodItems");

            migrationBuilder.AddColumn<string>(
                name: "BannerUrl",
                table: "Restaurants",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LogoUrl",
                table: "Restaurants",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhotoUrl",
                table: "FoodItems",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BannerUrl",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "LogoUrl",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "PhotoUrl",
                table: "FoodItems");

            migrationBuilder.AddColumn<int>(
                name: "BannerId",
                table: "Restaurants",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LogoId",
                table: "Restaurants",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PhotoId",
                table: "FoodItems",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Photo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PublicId = table.Column<string>(type: "text", nullable: true),
                    Url = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Photo", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Restaurants_BannerId",
                table: "Restaurants",
                column: "BannerId");

            migrationBuilder.CreateIndex(
                name: "IX_Restaurants_LogoId",
                table: "Restaurants",
                column: "LogoId");

            migrationBuilder.CreateIndex(
                name: "IX_FoodItems_PhotoId",
                table: "FoodItems",
                column: "PhotoId");

            migrationBuilder.AddForeignKey(
                name: "FK_FoodItems_Photo_PhotoId",
                table: "FoodItems",
                column: "PhotoId",
                principalTable: "Photo",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Restaurants_Photo_BannerId",
                table: "Restaurants",
                column: "BannerId",
                principalTable: "Photo",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Restaurants_Photo_LogoId",
                table: "Restaurants",
                column: "LogoId",
                principalTable: "Photo",
                principalColumn: "Id");
        }
    }
}
