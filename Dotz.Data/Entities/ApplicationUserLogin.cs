using Microsoft.AspNetCore.Identity;
using System;

namespace Dotz.Data.Entities
{
    public class ApplicationUserLogin : IdentityUserLogin<Guid>
    {
        public Guid Id { get; set; }
    }
}