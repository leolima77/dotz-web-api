using Dotz.Common.Enums;
using System;

namespace Dotz.Common.Dto
{
    public class DtoBase
    {
        public Guid Id { get; set; }

        public DateTime? CreateTimestamp { get; set; }
        public Guid? Creator { get; set; }
        public Status? Status { get; set; }
    }
}