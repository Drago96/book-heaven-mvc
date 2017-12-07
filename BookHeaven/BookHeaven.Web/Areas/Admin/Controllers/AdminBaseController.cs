using BookHeaven.Web.Infrastructure.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookHeaven.Web.Areas.Admin.Controllers
{
    [Area(AreaConstants.Admin)]
    [Authorize(Roles = RoleConstants.Admin)]
    public class AdminBaseController : Controller
    {
    }
}