﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BookHeaven.Services.UtilityServices.ShoppingCart.Models;

namespace BookHeaven.Services.Contracts
{
    public interface IOrderService : IService
    {
        Task FinishOrder(string userId, IDictionary<int, int> items);
    }
}
