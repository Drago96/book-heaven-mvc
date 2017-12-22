using BookHeaven.Services.Models.Orders;
using BookHeaven.Services.Models.Users;
using System.Collections.Generic;

namespace BookHeaven.Web.Models.Account
{
    public class UserProfileViewModel
    {
        public UserDetailsServiceModel User { get; set; }

        public IEnumerable<OrderDetailsServiceModel> Orders { get; set; }
    }
}