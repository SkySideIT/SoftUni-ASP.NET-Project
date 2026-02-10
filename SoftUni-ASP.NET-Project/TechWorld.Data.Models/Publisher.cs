using System.ComponentModel.DataAnnotations;
using static TechWorld.GCommon.EntityValidations;

namespace TechWorld.Data.Models
{
    public class Publisher
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(PublisherNameMaxLength)]
        public string Name { get; set; } = null!;

        [MaxLength(PublisherCountryMaxLength)]
        public string? Country { get; set; }

        [MaxLength(PublisherWebsiteMaxLength)]
        public string? Website { get; set; }

        public virtual ICollection<Game>? Games { get; set; }
            = new List<Game>();
    }
}
