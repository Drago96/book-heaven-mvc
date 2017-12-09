using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookHeaven.Web.Infrastructure.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookHeaven.Web.Areas.Publisher.Controllers
{
    [Area(AreaConstants.Publisher)]
    [Authorize(Roles = RoleConstants.Publisher)]
    public class PublisherBaseController : Controller
    {
    }
}
