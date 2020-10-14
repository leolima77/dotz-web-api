using Dotz.Common.Enums;
using Microsoft.AspNetCore.Identity;
using System;

namespace Dotz.Data.Entities
{
    public class ApplicationRole : IdentityRole<Guid>
    {
        private Status _status;
        private DateTime _createTimestamp;

        public string Description { get; set; }

        public DateTime? CreateTimestamp
        {
            get
            {
                return _createTimestamp;
            }
            set
            {
                _createTimestamp = value ?? DateTime.UtcNow;
            }
        }

        public Guid? Creator { get; set; }

        public Status? Status
        {
            get
            {
                return _status;
            }
            set
            {
                _status = value ?? Common.Enums.Status.ACTIVE;
            }
        }
    }
}