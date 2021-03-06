﻿namespace BookHeaven.Web.Infrastructure.Constants.ErrorMessages
{
    public static class UserErrorConstants
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

        public const string ExternalLoginInsufficientCredentials =
            "External user doesn't have all the credentials required to sign up.";

        public const string LogOutFirst = "You need to logout first!";
        public const string UserAlreadyExists = "Email is already taken!";
        public const string UserDoesNotExist = "User does not exist!";
        public const string ErrorChaningPassword = "There was an error changing your password.";
        public const string ErrorSettingPassword = "There was an error setting your password.";
    }
}