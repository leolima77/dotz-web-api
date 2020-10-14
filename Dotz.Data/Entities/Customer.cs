using Dotz.Common.Data;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Dotz.Data.Entities
{
    public class Customer : EntityBase
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, MinimumLength = 3,
        ErrorMessage = "Name Should be minimum 3 characters and a maximum 100 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Surname is required")]
        [StringLength(100, MinimumLength = 3,
        ErrorMessage = "Surname Should be minimum 3 characters and a maximum 100 characters")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Organization is required")]
        [DisplayName(nameof(Organization))]
        public Guid OrganizationId { get; set; }

        public virtual Organization Organization { get; set; }

        [Required(ErrorMessage = "Mail Address is required")]
        [StringLength(100, MinimumLength = 3,
        ErrorMessage = "Mail Address Should be minimum 3 characters and a maximum 100 characters")]
        [DataType(DataType.EmailAddress)]
        public string MailAdress { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        public string PasswordHash { get; set; }
    }
}