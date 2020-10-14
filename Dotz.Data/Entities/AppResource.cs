using Dotz.Common.Data;
using System;
using System.ComponentModel.DataAnnotations;

namespace Dotz.Data.Entities
{
    public class AppResource : EntityBase
    {
        [Required(ErrorMessage = "Key is required")]
        public string Key { get; set; }

        public Guid LanguageId { get; set; }

        public virtual Language Language { get; set; }

        [Required(ErrorMessage = "Value is required")]
        public string Value { get; set; }
    }
}