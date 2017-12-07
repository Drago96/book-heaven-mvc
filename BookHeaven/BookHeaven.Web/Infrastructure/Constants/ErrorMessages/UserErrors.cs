namespace BookHeaven.Web.Infrastructure.Constants.ErrorMessages
{
    public static class UserErrors
    {
        public const string PasswordsMustMatch = "The password and confirmation password do not match.";
        public const string InvalidEmail = "Input is not a valid email.";

        public const string InvalidProfilePicture =
            "The selected image was invalid. File must be a valid image with size less than 2MB.";

        public const string UserExists = "User already exists.";
        public const string ErrorCreatingUser = "There was an error creating your account.";
        public const string InvalidLoginAttempt = "Invalid login attempt.";
        public const string ErrorFromExternalProvider = "Error from external provider: {0}";

        public const string ExternalLoginInformation =
            "Error loading external login information during confirmation.";

        public const string ExternalLoginInsufficientCredentials = "External user doesn't have all the credentials required to sign up.";
    }
}