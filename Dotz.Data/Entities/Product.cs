using Dotz.Common.Data;
using System;

namespace Dotz.Data.Entities
{
    public class Product : EntityBase
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public Guid OrganizationId { get; set; }

        public Guid LanguageId { get; set; }
    }
}