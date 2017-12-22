using BookHeaven.Common.Mapping;
using BookHeaven.Data.Models;

namespace BookHeaven.Services.Models.Users
{
    public class UserNavServiceModel : IMapFrom<User>
    {
        public string ProfilePictureNav { get; set; }
    }
}