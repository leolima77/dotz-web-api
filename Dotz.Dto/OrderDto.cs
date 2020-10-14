using Dotz.Common.Dto;
using System.Collections.Generic;

namespace Dotz.Dto
{
    public class OrderDto : DtoBase
    {
        public CustomerDto Customer { get; set; }

        public List<ProductDto> Products { get; set; }

        public decimal Discount { get; set; }
    }
}