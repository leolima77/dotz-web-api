using Dotz.Common.Data;
using System.ComponentModel.DataAnnotations;

namespace Dotz.Data.Entities
{
    public class Organization : EntityBase
    {
        [Required(ErrorMessage = "Key is required")]
        [StringLength(100, MinimumLength = 3,
        ErrorMessage = "Name Should be minimum 3 characters and a maximum 100 characters")]
        public string Name { get; set; }

        public string TaxNumber { get; set; }

        public string TaxOffice { get; set; }

        public string Address { get; set; }
    }
}