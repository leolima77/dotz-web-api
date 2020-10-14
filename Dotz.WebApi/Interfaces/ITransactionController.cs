using Dotz.Common.Api.Base;
using Dotz.Data.Entities;
using Dotz.Dto;

namespace Dotz.WebApi.Interfaces
{
    public interface ITransactionController : IApiController<TransactionDto, Transaction>
    {
    }
}