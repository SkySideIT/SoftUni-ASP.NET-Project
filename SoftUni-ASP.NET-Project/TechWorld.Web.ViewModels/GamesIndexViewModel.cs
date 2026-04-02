namespace TechWorld.Web.ViewModels
{
    public class GamesIndexViewModel
    {
        public IEnumerable<GameDetailsViewModel> Games { get; set; } = null!;
        public IEnumerable<SelectGameGenreViewModel> Genres { get; set; } = null!;
        public IEnumerable<SelectGamePlatformViewModel> Platforms { get; set; } = null!;

        public string? SearchTerm { get; set; }
        public int? GenreId { get; set; }
        public int? PlatformId { get; set; }
    }
}
