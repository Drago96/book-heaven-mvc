using BookHeaven.Common.Mapping;
using BookHeaven.Data.Models;

namespace BookHeaven.Services.Models.Users
{
    public class UserAdminListingServiceModel : IMapFrom<User>
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }
        
        public string Email { get; set; }
    }
}
