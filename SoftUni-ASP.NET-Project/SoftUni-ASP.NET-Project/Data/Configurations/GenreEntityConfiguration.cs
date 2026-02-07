using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SoftUni_ASP.NET_Project.Models;

namespace SoftUni_ASP.NET_Project.Data.Configurations
{
    public class GenreEntityConfiguration : IEntityTypeConfiguration<Genre>
    {
        private readonly Genre[] GameGenres = new Genre[]
        {
            new Genre { Id = 1, Name = "Action" },
            new Genre { Id = 2, Name = "Adventure" },
            new Genre { Id = 3, Name = "Role-Playing" },
            new Genre { Id = 4, Name = "Simulation" },
            new Genre { Id = 5, Name = "Strategy" },
            new Genre { Id = 6, Name = "Sports" },
            new Genre { Id = 7, Name = "Puzzle" },
            new Genre { Id = 8, Name = "Idle" }
        };

        public void Configure(EntityTypeBuilder<Genre> entity)
        {
            entity.HasData(GameGenres);
        }
    }
}
