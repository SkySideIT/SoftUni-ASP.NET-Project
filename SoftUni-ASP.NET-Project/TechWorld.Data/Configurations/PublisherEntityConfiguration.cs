using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TechWorld.Data.Models;

namespace TechWorld.Web.Data.Configurations
{
    public class PublisherEntityConfiguration : IEntityTypeConfiguration<Publisher>
    {
        private readonly Publisher[] Publishers = new Publisher[]
        {
            new Publisher { Id = 1, Name = "Bandai Namco", Country = "Japan", Website = "https://www.bandainamco.com" },
            new Publisher { Id = 2, Name = "CD Projekt Red", Country = "Poland", Website = "https://www.cdprojekt.com" },
            new Publisher { Id = 3, Name = "Blizzard Entertainment", Country = "USA", Website = "https://www.blizzard.com" }
        };

        public void Configure(EntityTypeBuilder<Publisher> entity)
        {
            entity.HasData(Publishers);
        }
    }
}
