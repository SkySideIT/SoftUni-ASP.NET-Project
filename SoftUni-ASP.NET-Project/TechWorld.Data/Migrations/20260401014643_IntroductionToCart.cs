using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TechWorld.Data.Migrations
{
    /// <inheritdoc />
    public partial class IntroductionToCart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Games",
                keyColumn: "Id",
                keyValue: new Guid("67baf37e-abc4-4761-947f-e6fc13c3f0dd"));

            migrationBuilder.DeleteData(
                table: "Games",
                keyColumn: "Id",
                keyValue: new Guid("8c192970-ba0b-44ef-bede-e576e322f3dc"));

            migrationBuilder.DeleteData(
                table: "Games",
                keyColumn: "Id",
                keyValue: new Guid("989e18af-0b39-40bb-8ade-cb196c2ded9b"));

            migrationBuilder.DeleteData(
                table: "Games",
                keyColumn: "Id",
                keyValue: new Guid("a0b0d4ba-dfa9-4010-ba1f-29f666e8b8d6"));

            migrationBuilder.DeleteData(
                table: "Games",
                keyColumn: "Id",
                keyValue: new Guid("a84c0370-2f22-4611-a19b-c034a20f02ae"));

            migrationBuilder.DeleteData(
                table: "Games",
                keyColumn: "Id",
                keyValue: new Guid("e6bfb42d-613c-43d7-a813-cfdefa4c2a5e"));

            migrationBuilder.CreateTable(
                name: "CartProducts",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GameId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartProducts", x => new { x.UserId, x.GameId });
                    table.ForeignKey(
                        name: "FK_CartProducts_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CartProducts_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Games",
                columns: new[] { "Id", "Description", "GenreId", "ImageUrl", "PlatformId", "Price", "PublisherId", "ReleaseDate", "Title" },
                values: new object[,]
                {
                    { new Guid("06f071ac-d184-483e-b838-90b2cb505602"), "Team-based shooter developed and published by Blizzard Entertainment.", 2, "https://cdn.cloudflare.steamstatic.com/steam/apps/2357570/header.jpg", 2, 0.00m, 3, new DateOnly(2022, 10, 4), "Overwatch 2" },
                    { new Guid("0b20e18b-16e1-4649-a589-65a835318f2c"), "Fighting game developed and published by Bandai Namco.", 1, "https://cdn.cloudflare.steamstatic.com/steam/apps/1778820/header.jpg", 3, 59.99m, 1, new DateOnly(2024, 1, 26), "Tekken 8" },
                    { new Guid("18adeee5-6d0e-4f47-85c7-771878f9ea68"), "Open world fantasy RPG developed by CD Projekt Red.", 1, "https://cdn.cloudflare.steamstatic.com/steam/apps/292030/header.jpg", 1, 89.99m, 2, new DateOnly(2015, 5, 19), "The Witcher 3: Wild Hunt" },
                    { new Guid("6520dc2e-31d7-4087-9896-6bfb8c321bbe"), "Action RPG developed and published by Blizzard Entertainment.", 3, "https://cdn.cloudflare.steamstatic.com/steam/apps/2344520/header.jpg", 2, 69.99m, 3, new DateOnly(2023, 6, 6), "Diablo IV" },
                    { new Guid("8dcca106-4c8d-4500-8070-5383e9728e4d"), "Action RPG developed by FromSoftware and published by Bandai Namco.", 1, "https://cdn.cloudflare.steamstatic.com/steam/apps/1245620/header.jpg", 1, 119.99m, 1, new DateOnly(2022, 2, 25), "Elden Ring" },
                    { new Guid("f419c5ac-7228-4731-b021-f234603f9567"), "Futuristic open-world RPG developed by CD Projekt Red.", 3, "https://cdn.cloudflare.steamstatic.com/steam/apps/1091500/header.jpg", 1, 99.99m, 2, new DateOnly(2020, 12, 10), "Cyberpunk 2077" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CartProducts_GameId",
                table: "CartProducts",
                column: "GameId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CartProducts");

            migrationBuilder.DeleteData(
                table: "Games",
                keyColumn: "Id",
                keyValue: new Guid("06f071ac-d184-483e-b838-90b2cb505602"));

            migrationBuilder.DeleteData(
                table: "Games",
                keyColumn: "Id",
                keyValue: new Guid("0b20e18b-16e1-4649-a589-65a835318f2c"));

            migrationBuilder.DeleteData(
                table: "Games",
                keyColumn: "Id",
                keyValue: new Guid("18adeee5-6d0e-4f47-85c7-771878f9ea68"));

            migrationBuilder.DeleteData(
                table: "Games",
                keyColumn: "Id",
                keyValue: new Guid("6520dc2e-31d7-4087-9896-6bfb8c321bbe"));

            migrationBuilder.DeleteData(
                table: "Games",
                keyColumn: "Id",
                keyValue: new Guid("8dcca106-4c8d-4500-8070-5383e9728e4d"));

            migrationBuilder.DeleteData(
                table: "Games",
                keyColumn: "Id",
                keyValue: new Guid("f419c5ac-7228-4731-b021-f234603f9567"));

            migrationBuilder.InsertData(
                table: "Games",
                columns: new[] { "Id", "Description", "GenreId", "ImageUrl", "PlatformId", "Price", "PublisherId", "ReleaseDate", "Title" },
                values: new object[,]
                {
                    { new Guid("67baf37e-abc4-4761-947f-e6fc13c3f0dd"), "Fighting game developed and published by Bandai Namco.", 1, "https://cdn.cloudflare.steamstatic.com/steam/apps/1778820/header.jpg", 3, 59.99m, 1, new DateOnly(2024, 1, 26), "Tekken 8" },
                    { new Guid("8c192970-ba0b-44ef-bede-e576e322f3dc"), "Action RPG developed by FromSoftware and published by Bandai Namco.", 1, "https://cdn.cloudflare.steamstatic.com/steam/apps/1245620/header.jpg", 1, 119.99m, 1, new DateOnly(2022, 2, 25), "Elden Ring" },
                    { new Guid("989e18af-0b39-40bb-8ade-cb196c2ded9b"), "Open world fantasy RPG developed by CD Projekt Red.", 1, "https://cdn.cloudflare.steamstatic.com/steam/apps/292030/header.jpg", 1, 89.99m, 2, new DateOnly(2015, 5, 19), "The Witcher 3: Wild Hunt" },
                    { new Guid("a0b0d4ba-dfa9-4010-ba1f-29f666e8b8d6"), "Action RPG developed and published by Blizzard Entertainment.", 3, "https://cdn.cloudflare.steamstatic.com/steam/apps/2344520/header.jpg", 2, 69.99m, 3, new DateOnly(2023, 6, 6), "Diablo IV" },
                    { new Guid("a84c0370-2f22-4611-a19b-c034a20f02ae"), "Team-based shooter developed and published by Blizzard Entertainment.", 2, "https://cdn.cloudflare.steamstatic.com/steam/apps/2357570/header.jpg", 2, 0.00m, 3, new DateOnly(2022, 10, 4), "Overwatch 2" },
                    { new Guid("e6bfb42d-613c-43d7-a813-cfdefa4c2a5e"), "Futuristic open-world RPG developed by CD Projekt Red.", 3, "https://cdn.cloudflare.steamstatic.com/steam/apps/1091500/header.jpg", 1, 99.99m, 2, new DateOnly(2020, 12, 10), "Cyberpunk 2077" }
                });
        }
    }
}
