using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static BookHeaven.Data.Infrastructure.Constants.UserDataConstants;

namespace BookHeaven.Data.Models
{
    public class User : IdentityUser
    {
        [Required]
        [MinLength(FirstNameMinLength)]
        [MaxLength(FirstNameMaxLength)]
        public string FirstName { get; set; }

        [Required]
        [MinLength(LastNameMinLength)]
        [MaxLength(LastNameMaxLength)]
        public string LastName { get; set; }

        [MaxLength(ProfilePictureMaxLength)]
        public byte[] ProfilePicture { get; set; }

        public ICollection<Book> PublishedBooks { get; set; } = new List<Book>();

        public ICollection<ShoppingCartItem> ShoppingCartItems { get; set; } = new List<ShoppingCartItem>();
    }
}