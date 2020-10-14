using Dotz.Common.Enums;
using Microsoft.AspNetCore.Identity;
using System;

namespace Dotz.Data.Entities
{
    public class ApplicationUserRole : IdentityUserRole<Guid>
    {
        private Status _status;
        private DateTime _createTimestamp;

        public Guid Id { get; set; }

        public virtual ApplicationRole Role { get; set; }

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
                _status = value ?? Dotz.Common.Enums.Status.ACTIVE;
            }
        }
    }
}