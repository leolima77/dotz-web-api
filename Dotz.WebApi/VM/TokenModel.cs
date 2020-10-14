using Dotz.Dto;

namespace Dotz.WebApi.VM
{
    public class TokenModel
    {
        public object Token { get; set; }
        public ApplicationUserDto UserDto { get; set; }
    }
}
