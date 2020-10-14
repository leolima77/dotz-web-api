using Dotz.Common.Api;
using Dotz.Common.Api.Base;
using Dotz.Data.Entities;
using Dotz.Dto;
using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Dotz.Common.Paging;

namespace Dotz.WebApi.Controllers
{
    [ApiController]
    [Route("Customer")]
    public class CustomerController : ApiBase<Customer, CustomerDto, CustomerController>
    {
        public CustomerController(IServiceProvider service) : base(service)
        {
        }

        //public override ApiResult<CustomerDto> Find(Guid id)
        //{
        //    return new ApiResult<CustomerDto>
        //    {
        //        StatusCode = StatusCodes.Status200OK,
        //        Message = "User founded",
        //        Body = Mapper.Map<Customer, CustomerDto>(GetQueryable().Include(x => x.Organization).FirstOrDefault(x => x.Id == id))
        //    };
        //}

        public override ApiResult<CustomerDto> Add([FromBody] CustomerDto item)
        {
            var result = base.Add(item);
            _uow.SaveChanges(false);
            return result;
        }

        public override ApiResult<CustomerDto> Update([FromBody] CustomerDto item)
        {
            var result = base.Update(item);
            _uow.SaveChanges(true);
            return result;
        }

        public override ApiResult<string> Delete([FromBody] CustomerDto item)
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
