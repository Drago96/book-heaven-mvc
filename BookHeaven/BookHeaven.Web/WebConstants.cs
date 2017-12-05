namespace BookHeaven.Web
{
    public static class WebConstants
    {
        public const string InvalidParameterLengthErrorMessage =
            "{0} must be at least {2} and at max {1} characters long.";
        public const string PasswordsMustMatchErrorMessage = "The password and confirmation password do not match.";
        public const string EmailErrorMessage = "Input is not a valid email.";

        public const string UserFirstNameDisplay = "First Name";
        public const string UserLastNameDisplay = "Last Name";
        public const string UserConfirmPasswordDisplay = "Confirm Password";

        public const string DefaultProfilePictureUrl = "/images/user-images/defaultUser.png";
        public const string HomePageBookStoreImageUrl = "/images/home-page-images/bookstore.jpg";
    }
}
