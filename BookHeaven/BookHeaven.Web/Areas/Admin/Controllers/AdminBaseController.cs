using BookHeaven.Web.Infrastructure.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookHeaven.Web.Areas.Admin.Controllers
{
    [Area(Infrastructure.Constants.Areas.Admin)]
    [Authorize(Roles = Roles.Admin)]
    public class AdminBaseController : Controller
    {
    }
}