using Dotz.Common.Enums;
using Microsoft.AspNetCore.Http;
using System;

namespace Dotz.Common.Data
{
    public class EntityBase
    {

        private Status _status;
        private DateTime _createdTimestamp;
        private DateTime _updatedTimestamp;
        private Guid? _creator;

        public Guid Id { get; set; }

        public DateTime? CreateTimestamp
        {
            get
            {
                return _createdTimestamp;
            }
            set
            {
                _createdTimestamp = value ?? DateTime.UtcNow;
            }
        }

        public DateTime? UpdateTimestamp
        {
            get
            {
                return _updatedTimestamp;
            }
            set
            {
                _updatedTimestamp = value ?? DateTime.UtcNow;
            }
        }

        public Guid? Creator
        {
            get
            {
                return _creator;
            }
            set
            {
                if (value != null)
                {
                    HttpContextAccessor contextAccessor = new HttpContextAccessor();
                    var userClaim = contextAccessor.HttpContext.User.FindFirst("jti");
                    Guid.TryParse(userClaim?.Value, out Guid userId);
                    _creator = value ?? userId;
                }
            }
        }

        public Status? Status
        {
            get
            {
                return _status;
            }
            set
            {
                _status = value ?? Enums.Status.ACTIVE;
            }
        }
    }
}
