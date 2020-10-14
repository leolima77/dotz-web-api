using Dotz.Common.Dto;
using Dotz.Data.Entities;
using System;

namespace Dotz.Dto
{
    public class ApplicationUserRoleDto : DtoBase
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public ApplicationRoleDto Role { get; set; }

        public Guid RoleId { get; set; }
    }
}