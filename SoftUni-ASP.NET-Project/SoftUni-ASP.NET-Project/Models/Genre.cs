using System.ComponentModel.DataAnnotations;
using static SoftUni_ASP.NET_Project.Common.EntityValidations;

namespace SoftUni_ASP.NET_Project.Models
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
