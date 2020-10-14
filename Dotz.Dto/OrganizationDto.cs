using Dotz.Common.Dto;

namespace Dotz.Dto
{
    public class OrganizationDto : DtoBase
    {
        public string Name { get; set; }

        public string TaxNumber { get; set; }

        public string TaxOffice { get; set; }

        public string Address { get; set; }
    }
}