using Dotz.Common.Dto;
using System;

namespace Dotz.Dto
{
    public class CustomerDto : DtoBase
    {
        public string Name { get; set; }

        public string Surname { get; set; }

        public Guid OrganizationId { get; set; }

        public OrganizationDto Organization { get; set; }

        public string MailAdress { get; set; }

        public string Phone { get; set; }
    }
}