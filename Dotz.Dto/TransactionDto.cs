using Dotz.Common.Dto;
using Dotz.Common.Enums;

namespace Dotz.Dto
{
    public class TransactionDto : DtoBase
    {
        public Cash CashInOut { get; set; }

        public decimal Amount { get; set; }

        public CustomerDto Customer { get; set; }
    }
}
