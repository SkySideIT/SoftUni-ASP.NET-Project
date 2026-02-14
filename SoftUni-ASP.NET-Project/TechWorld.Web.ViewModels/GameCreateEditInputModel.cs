using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TechWorld.GCommon.EntityValidations;

namespace TechWorld.Web.ViewModels
{
    public class GameCreateEditInputModel
    {
        [Required]
        [MinLength(GameTitleMinLength)]
        [MaxLength(GameTitleMaxLength)]
        public string Title { get; set; } = null!;

        [Required]
        [MinLength(GameDescriptionMinLength)]
        [MaxLength(GameDescriptionMaxLength)]
        public string Description { get; set; } = null!;

        [Required]
        [Range(0.00, 500)]
        public decimal Price { get; set; }

        [Required]
        public DateOnly ReleaseDate { get; set; }

        [Required]
        [Url]
        public string ImageUrl { get; set; } = null!;

        [Required]
        [Display(Name = "Genre")]
        public int GenreId { get; set; }

        [Required]
        [Display(Name = "Platform")]
        public int PlatformId { get; set; }

        [Required]
        [Display(Name = "Publisher")]
        public string PublisherName { get; set; } = null!;

        public IEnumerable<SelectGameGenreViewModel> Genres { get; set; }
            = new List<SelectGameGenreViewModel>();
        public IEnumerable<SelectGamePlatformViewModel> Platforms { get; set; }
            = new List<SelectGamePlatformViewModel>();
    }
}
