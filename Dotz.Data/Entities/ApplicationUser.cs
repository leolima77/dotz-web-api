using Dotz.Common.Enums;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Dotz.Data.Entities
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        private Status _status;
        private DateTime _createTimestamp;

        public string Title { get; set; }

        public virtual List<ApplicationUserRole> UserRoles { get; set; }

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

        public Guid LanguageId { get; set; }
    }
}