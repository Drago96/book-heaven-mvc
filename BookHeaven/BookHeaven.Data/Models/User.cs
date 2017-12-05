namespace BookHeaven.Data.Models
{
    using Microsoft.AspNetCore.Identity;
    using System.ComponentModel.DataAnnotations;

    using static DataConstants;

    public class User : IdentityUser
    {
        [Required]
        [MinLength(UserFirstNameMinLength)]
        [MaxLength(UserFirstNameMaxLength)]
        public string FirstName { get; set; }

        [Required]
        [MinLength(UserLastNameMinLength)]
        [MaxLength(UserFirstNameMaxLength)]
        public string LastName { get; set; }

        [MaxLength(UserProfilePictureMaxLength)]
        public byte[] ProfilePicture { get; set; }
    }
}
