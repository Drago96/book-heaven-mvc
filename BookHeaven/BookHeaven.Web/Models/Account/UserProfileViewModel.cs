using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookHeaven.Services.Models.Orders;
using BookHeaven.Services.Models.Users;

namespace BookHeaven.Web.Models.Account
{
    public class UserProfileViewModel
    {
        public UserDetailsServiceModel User { get; set; }

        public IEnumerable<OrderDetailsServiceModel> Orders { get; set; }
    }
}
