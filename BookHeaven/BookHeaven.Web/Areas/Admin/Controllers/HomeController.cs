using Microsoft.AspNetCore.Mvc;

namespace BookHeaven.Web.Areas.Admin.Controllers
{
    public class HomeController : AdminBaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}