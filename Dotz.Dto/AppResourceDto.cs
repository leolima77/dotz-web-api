using Dotz.Common.Dto;
using System;

namespace Dotz.Dto
{
    public class AppResourceDto : DtoBase
    {
        public string Key { get; set; }

        public Guid LanguageId { get; set; }

        public LanguageDto Language { get; set; }

        public string Value { get; set; }
    }
}