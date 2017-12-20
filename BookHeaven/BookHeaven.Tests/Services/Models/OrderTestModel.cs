using System;
using System.Collections.Generic;
using System.Text;
using BookHeaven.Common.Mapping;
using BookHeaven.Data.Models;

namespace BookHeaven.Tests.Services.Models
{
    public class OrderTestModel : IMapFrom<Order>
    {
        public int Id { get; set; }

        public string UserId { get; set; }
    }
}
