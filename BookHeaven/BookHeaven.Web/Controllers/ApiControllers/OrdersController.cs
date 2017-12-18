using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BookHeaven.Services.Contracts;
using BookHeaven.Web.Infrastructure.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookHeaven.Web.Controllers.ApiControllers
{
    public class OrdersController : BaseApiController
    {
        private readonly IOrderService orders;

        public OrdersController(IOrderService orders)
        {
            this.orders = orders;
        }

        [HttpGet]
        [Authorize(Roles = RoleConstants.Publisher)]
        public async Task<IActionResult> GetSalesForYear(int year)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var sales = await this.orders.SalesForYear(year, userId);

            return Ok(sales);
        }
    }
}
