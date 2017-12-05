namespace BookHeaven.Data
{
    public static class DataConstants
    {
        public const int UserFirstNameMinLength = 2;
        public const int UserFirstNameMaxLength = 50;

        public const int UserLastNameMinLength = 2;
        public const int UserLastNameMaxLength = 50;

        public const int UserPasswordMinLength = 6;
        public const int UserPasswordMaxLength = 100;

        public const int UserProfilePictureMaxLength = 2 * 1024 * 1024;

        public const int UserProfilePictureWidth = 150;
        public const int UserProfilePictureHeight = 150;
    }
}
