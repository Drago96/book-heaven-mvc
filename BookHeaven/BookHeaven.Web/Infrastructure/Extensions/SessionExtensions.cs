using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookHeaven.Web.Infrastructure.Constants;
using Microsoft.AspNetCore.Http;

namespace BookHeaven.Web.Infrastructure.Extensions
{
    public static class SessionExtensions
    {
        public static string GetShoppingCartId(this ISession session)
        {
            var shoppingCartId = session.GetString(SessionConstants.ShoppingCartKey);

            if (shoppingCartId == null)
            {
                shoppingCartId = Guid.NewGuid().ToString();
                session.SetString(SessionConstants.ShoppingCartKey, shoppingCartId);
            }

            return shoppingCartId;
        }
    }
}
