using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TechWorld.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Genres",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genres", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Platforms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Platforms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Publishers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Country = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Website = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Publishers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    ReleaseDate = table.Column<DateOnly>(type: "date", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: true),
                    GenreId = table.Column<int>(type: "int", nullable: false),
                    PlatformId = table.Column<int>(type: "int", nullable: false),
                    PublisherId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Games_Genres_GenreId",
                        column: x => x.GenreId,
                        principalTable: "Genres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Games_Platforms_PlatformId",
                        column: x => x.PlatformId,
                        principalTable: "Platforms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Games_Publishers_PublisherId",
                        column: x => x.PublisherId,
                        principalTable: "Publishers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Genres",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Action" },
                    { 2, "Adventure" },
                    { 3, "Role-Playing" },
                    { 4, "Simulation" },
                    { 5, "Strategy" },
                    { 6, "Sports" },
                    { 7, "Puzzle" },
                    { 8, "Idle" }
                });

            migrationBuilder.InsertData(
                table: "Platforms",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "PC" },
                    { 2, "PlayStation 5" },
                    { 3, "Xbox Series X" }
                });

            migrationBuilder.InsertData(
                table: "Publishers",
                columns: new[] { "Id", "Country", "Name", "Website" },
                values: new object[,]
                {
                    { 1, "Japan", "Bandai Namco", "https://www.bandainamco.com" },
                    { 2, "Poland", "CD Projekt Red", "https://www.cdprojekt.com" },
                    { 3, "USA", "Blizzard Entertainment", "https://www.blizzard.com" }
                });

            migrationBuilder.InsertData(
                table: "Games",
                columns: new[] { "Id", "Description", "GenreId", "ImageUrl", "PlatformId", "Price", "PublisherId", "ReleaseDate", "Title" },
                values: new object[,]
                {
                    { new Guid("6f7eb41a-9f63-44a3-8d66-7065b7e1b27e"), "Team-based shooter developed and published by Blizzard Entertainment.", 2, "https://cdn.cloudflare.steamstatic.com/steam/apps/2357570/header.jpg", 2, 0.00m, 3, new DateOnly(2022, 10, 4), "Overwatch 2" },
                    { new Guid("b391bec7-115f-40c6-866c-68b52eaaa777"), "Action RPG developed by FromSoftware and published by Bandai Namco.", 1, "https://cdn.cloudflare.steamstatic.com/steam/apps/1245620/header.jpg", 1, 119.99m, 1, new DateOnly(2022, 2, 25), "Elden Ring" },
                    { new Guid("bc3b946e-e9ef-4822-86e9-90fa6309c1a4"), "Open world fantasy RPG developed by CD Projekt Red.", 1, "https://cdn.cloudflare.steamstatic.com/steam/apps/292030/header.jpg", 1, 89.99m, 2, new DateOnly(2015, 5, 19), "The Witcher 3: Wild Hunt" },
                    { new Guid("b7036922-99a0-471e-86f8-fbc0bea7061b"), "Futuristic open-world RPG developed by CD Projekt Red.", 3, "https://cdn.cloudflare.steamstatic.com/steam/apps/1091500/header.jpg", 1, 99.99m, 2, new DateOnly(2020, 12, 10), "Cyberpunk 2077" },
                    { new Guid("007641f1-5b91-4cc4-8113-82a87e42a2f7"), "Action RPG developed and published by Blizzard Entertainment.", 3, "https://cdn.cloudflare.steamstatic.com/steam/apps/2344520/header.jpg", 2, 69.99m, 3, new DateOnly(2023, 6, 6), "Diablo IV" },
                    { new Guid("91d9d4ea-cdb0-44b9-8b3a-7d81a3c41ea1"), "Fighting game developed and published by Bandai Namco.", 1, "https://cdn.cloudflare.steamstatic.com/steam/apps/1778820/header.jpg", 3, 59.99m, 1, new DateOnly(2024, 1, 26), "Tekken 8" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Games_GenreId",
                table: "Games",
                column: "GenreId");

            migrationBuilder.CreateIndex(
                name: "IX_Games_PlatformId",
                table: "Games",
                column: "PlatformId");

            migrationBuilder.CreateIndex(
                name: "IX_Games_PublisherId",
                table: "Games",
                column: "PublisherId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Games");

            migrationBuilder.DropTable(
                name: "Genres");

            migrationBuilder.DropTable(
                name: "Platforms");

            migrationBuilder.DropTable(
                name: "Publishers");
        }
    }
}
