using System;

namespace Dotz.WebApi.VM
{
    public class ChangePasswordModel
    {
        public Guid? UserId { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}