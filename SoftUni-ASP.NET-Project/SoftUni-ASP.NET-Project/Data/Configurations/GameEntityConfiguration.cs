using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SoftUni_ASP.NET_Project.Models;

namespace SoftUni_ASP.NET_Project.Data.Configurations
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
                 ImageUrl = "https://example.com/images/elden-ring.jpg",
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
                 ImageUrl = "https://example.com/images/witcher3.jpg",
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
                 ImageUrl = "https://example.com/images/overwatch2.jpg",
                 GenreId = 2,
                 PlatformId = 2,
                 PublisherId = 3
            }
        };

        public void Configure(EntityTypeBuilder<Game> entity)
        {
            entity.HasData(Games);
        }
    }
}
