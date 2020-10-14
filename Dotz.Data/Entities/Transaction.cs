using Dotz.Common.Data;
using Dotz.Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dotz.Data.Entities
{
    public class Transaction : EntityBase
    {
        public Cash CashInOut { get; set; }

        public decimal Amount { get; set; }

        public Guid CustomerId { get; set; }
    }
}
