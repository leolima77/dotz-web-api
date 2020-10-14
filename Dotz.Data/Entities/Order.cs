using Dotz.Common.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Dotz.Data.Entities
{
    public class Order : EntityBase
    {
        public Guid CustomerId { get; set; }

        public virtual List<Product> Products { get; set; }

        public decimal Discount { get; set; }
    }
}