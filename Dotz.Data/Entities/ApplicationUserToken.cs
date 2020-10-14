using Microsoft.AspNetCore.Identity;
using System;

namespace Dotz.Data.Entities
{
    public class ApplicationUserToken : IdentityUserToken<Guid>
    {
        public Guid Id { get; set; }
    }
}