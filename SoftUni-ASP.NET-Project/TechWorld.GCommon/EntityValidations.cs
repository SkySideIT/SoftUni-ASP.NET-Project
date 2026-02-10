namespace TechWorld.GCommon
{
    public static class EntityValidations
    {
        /* Game */
        public const int GameTitleMinLength = 2;
        public const int GameTitleMaxLength = 100;
        public const int GameDescriptionMinLength = 20;
        public const int GameDescriptionMaxLength = 512;
        public const int GameImageUrlMinLength = 10;
        public const int GameImageUrlMaxLength = 2048;

        /* Genre */
        public const int GenreNameMinLength = 2;
        public const int GenreNameMaxLength = 50;

        /* Platform */
        public const int PlatformNameMinLength = 2;
        public const int PlatformNameMaxLength = 50;

        /* Publisher */
        public const int PublisherNameMinLength = 2;
        public const int PublisherNameMaxLength = 100;
        public const int PublisherCountryMinLength = 2;
        public const int PublisherCountryMaxLength = 100;
        public const int PublisherWebsiteMinLength = 10;
        public const int PublisherWebsiteMaxLength = 2048;
    }
}
