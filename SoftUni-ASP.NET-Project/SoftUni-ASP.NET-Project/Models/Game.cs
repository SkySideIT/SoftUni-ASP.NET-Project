using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using static SoftUni_ASP.NET_Project.Common.EntityValidations;
using static SoftUni_ASP.NET_Project.Common.ApplicationConstants;

namespace SoftUni_ASP.NET_Project.Models
{
    public class Game
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(GameTitleMaxLength)]
        public string Title { get; set; } = null!;

        [Required]
        [MaxLength(GameDescriptionMaxLength)]
        public string Description { get; set; } = null!;

        [Column(TypeName = GamePrice)]
        public decimal Price { get; set; }

        public DateOnly ReleaseDate { get; set; }

        [MaxLength(GameImageUrlMaxLength)]
        public string? ImageUrl { get; set; }

        [Required]
        [ForeignKey(nameof(Genre))]
        public int GenreId { get; set; }

        public virtual Genre Genre { get; set; } = null!;

        [Required]
        [ForeignKey(nameof(Platform))]
        public int PlatformId { get; set; }

        public virtual Platform Platform { get; set; } = null!;

        [Required]
        [ForeignKey(nameof(Publisher))]
        public int PublisherId { get; set; }

        public virtual Publisher Publisher { get; set; } = null!;
    }
}
