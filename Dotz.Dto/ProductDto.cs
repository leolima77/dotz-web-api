using Dotz.Common.Dto;

namespace Dotz.Dto
{
    public class ProductDto : DtoBase
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }
    }
}