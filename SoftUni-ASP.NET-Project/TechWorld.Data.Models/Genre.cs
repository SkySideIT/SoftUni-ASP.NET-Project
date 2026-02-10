using System.ComponentModel.DataAnnotations;
using static TechWorld.GCommon.EntityValidations;

namespace TechWorld.Data.Models
{
    public class Genre
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(GenreNameMaxLength)]
        public string Name { get; set; } = null!;

        public virtual ICollection<Game> Games { get; set; } 
            = new List<Game>();
    }
}
