using Dotz.Common.Api;
using Dotz.WebApi.VM;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Dotz.WebApi.Interfaces
{
    public interface ITokenController
    {
        Task<ApiResult> LoginAsync([FromBody] LoginModel loginModel);
    }
}
