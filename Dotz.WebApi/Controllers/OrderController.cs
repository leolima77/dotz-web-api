using Dotz.Common.Api;
using Dotz.Common.Api.Base;
using Dotz.Data.Entities;
using Dotz.Dto;
using Dotz.WebApi.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Dotz.WebApi.Controllers
{
    [ApiController]
    [Route("Order")]
    public class OrderController : ApiBase<Order, OrderDto, OrderController>, IOrderController
    {
        #region Constructor

        public OrderController(IServiceProvider service) : base(service)
        {
        }

        #endregion Constructor

        public override ApiResult<OrderDto> Add([FromBody] OrderDto item)
        {
            var result = base.Add(item);
            _uow.SaveChanges(false);
            return result;
        }

        public override ApiResult<OrderDto> Update([FromBody] OrderDto item)
        {
            var result = base.Update(item);
            _uow.SaveChanges(true);
            return result;
        }

        public override ApiResult<string> Delete([FromBody] OrderDto item)
        {
            var result = base.Delete(item);
            _uow.SaveChanges(true);
            return result;
        }

        public override ApiResult<string> DeleteById(Guid id)
        {
            var result = base.DeleteById(id);
            _uow.SaveChanges(true);
            return result;
        }
    }
}