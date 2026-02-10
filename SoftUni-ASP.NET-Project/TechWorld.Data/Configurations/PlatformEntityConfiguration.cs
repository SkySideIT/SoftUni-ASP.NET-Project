using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TechWorld.Data.Models;

namespace TechWorld.Data.Configurations
{
    public class PlatformEntityConfiguration : IEntityTypeConfiguration<Platform>
    {
        private readonly Platform[] Platforms = new Platform[]
        {
            new Platform { Id = 1, Name = "PC" },
            new Platform { Id = 2, Name = "PlayStation 5" },
            new Platform { Id = 3, Name = "Xbox Series X" }
        };

        public void Configure(EntityTypeBuilder<Platform> entity)
        {
            entity.HasData(Platforms);
        }
    }
}
