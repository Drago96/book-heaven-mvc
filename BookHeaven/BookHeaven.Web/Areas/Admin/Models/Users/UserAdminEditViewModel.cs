using BookHeaven.Web.Models.Account;
using System.Collections.Generic;

namespace BookHeaven.Web.Areas.Admin.Models.Users
{
    public class UserAdminEditViewModel : UserEditViewModel
    {
        public IEnumerable<string> Roles { get; set; } = new List<string>();

        public IEnumerable<string> AllRoles { get; set; } = new List<string>();
    }
}