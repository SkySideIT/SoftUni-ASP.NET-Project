using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechWorld.Web.ViewModels
{
    public class GameDetailsViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public string Genre { get; set; } = null!;
        public string Platform { get; set; } = null!;
        public string Publisher { get; set; } = null!;
        public DateOnly ReleaseDate { get; set; }
        public string? ImageUrl { get; set; }
    }
}
