using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TechWorld.Data.Models;

namespace TechWorld.Data.Configurations
{
    public class GameEntityConfiguration : IEntityTypeConfiguration<Game>
    {
        private readonly Game[] Games = new Game[]
        {
            new Game
            {
                 Id = Guid.NewGuid(),
                 Title = "Elden Ring",
                 Description = "Action RPG developed by FromSoftware and published by Bandai Namco.",
                 Price = 119.99m,
                 ReleaseDate = DateOnly.FromDateTime(new DateTime(2022, 2, 25)),
                 ImageUrl = "https://cdn.cloudflare.steamstatic.com/steam/apps/1245620/header.jpg",
                 GenreId = 1,
                 PlatformId = 1,
                 PublisherId = 1
            },
            new Game
            {
                 Id = Guid.NewGuid(),
                 Title = "The Witcher 3: Wild Hunt",
                 Description = "Open world fantasy RPG developed by CD Projekt Red.",
                 Price = 89.99m,
                 ReleaseDate = DateOnly.FromDateTime(new DateTime(2015, 5, 19)),
                 ImageUrl = "https://cdn.cloudflare.steamstatic.com/steam/apps/292030/header.jpg",
                 GenreId = 1,
                 PlatformId = 1,
                 PublisherId = 2
            },
            new Game
            {
                 Id = Guid.NewGuid(),
                 Title = "Overwatch 2",
                 Description = "Team-based shooter developed and published by Blizzard Entertainment.",
                 Price = 0.00m,
                 ReleaseDate = DateOnly.FromDateTime(new DateTime(2022, 10, 4)),
                 ImageUrl = "https://cdn.cloudflare.steamstatic.com/steam/apps/2357570/header.jpg",
                 GenreId = 2,
                 PlatformId = 2,
                 PublisherId = 3
            },
            new Game
            {
                Id = Guid.NewGuid(),
                Title = "Cyberpunk 2077",
                Description = "Futuristic open-world RPG developed by CD Projekt Red.",
                Price = 99.99m,
                ReleaseDate = DateOnly.FromDateTime(new DateTime(2020, 12, 10)),
                ImageUrl = "https://cdn.cloudflare.steamstatic.com/steam/apps/1091500/header.jpg",
                GenreId = 3,
                PlatformId = 1,
                PublisherId = 2
            },
            new Game
            {
                Id = Guid.NewGuid(),
                Title = "Diablo IV",
                Description = "Action RPG developed and published by Blizzard Entertainment.",
                Price = 69.99m,
                ReleaseDate = DateOnly.FromDateTime(new DateTime(2023, 6, 6)),
                ImageUrl = "https://cdn.cloudflare.steamstatic.com/steam/apps/2344520/header.jpg",
                GenreId = 3,
                PlatformId = 2,
                PublisherId = 3
            },
            new Game
            {
                Id = Guid.NewGuid(),
                Title = "Tekken 8",
                Description = "Fighting game developed and published by Bandai Namco.",
                Price = 59.99m,
                ReleaseDate = DateOnly.FromDateTime(new DateTime(2024, 1, 26)),
                ImageUrl = "https://cdn.cloudflare.steamstatic.com/steam/apps/1778820/header.jpg",
                GenreId = 1,
                PlatformId = 3,
                PublisherId = 1
            },
        };

        public void Configure(EntityTypeBuilder<Game> entity)
        {
            entity.HasData(Games);
        }
    }
}
