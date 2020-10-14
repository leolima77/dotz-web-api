using Dotz.Common.Dto;
using System;
using System.Collections.Generic;

namespace Dotz.Dto
{

    public class ApplicationUserDto : DtoBase
    {
        public bool EmailConfirmed { get; set; }

        public string Email { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string PhoneNumber { get; set; }

        public bool PhoneNumberConfirmed { get; set; }

        public string Title { get; set; }

        public List<ApplicationUserRoleDto> UserRoles { get; set; }

        public Guid LanguageId { get; set; }
    }
}