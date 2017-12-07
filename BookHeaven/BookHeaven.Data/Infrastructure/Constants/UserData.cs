namespace BookHeaven.Data.Infrastructure.Constants
{
    public static class UserData
    {
        public const int FirstNameMinLength = 2;
        public const int FirstNameMaxLength = 50;

        public const int LastNameMinLength = 2;
        public const int LastNameMaxLength = 50;

        public const int PasswordMinLength = 6;
        public const int PasswordMaxLength = 100;

        public const int ProfilePictureMaxLength = 2 * 1024 * 1024;

        public const int ProfilePictureWidth = 150;
        public const int ProfilePictureHeight = 150;
    }
}