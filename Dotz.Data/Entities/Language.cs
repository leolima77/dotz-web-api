using Dotz.Common.Data;
using System.ComponentModel.DataAnnotations;

namespace Dotz.Data.Entities
{
    public class Language : EntityBase
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Culture is required")]
        public string Culture { get; set; }
    }
}