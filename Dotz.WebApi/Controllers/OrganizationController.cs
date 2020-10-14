using Dotz.Common.Api;
using Dotz.Common.Api.Base;
using Dotz.Data.Entities;
using Dotz.Dto;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Dotz.WebApi.Controllers
{
    [ApiController]
    [Route("Organization")]
    public class OrganizationController : ApiBase<Organization, OrganizationDto, OrganizationController>
    {
        public OrganizationController(IServiceProvider service) : base(service)
        {
        }

        public override ApiResult<OrganizationDto> Add([FromBody] OrganizationDto item)
        {
            var result = base.Add(item);
            _uow.SaveChanges(false);
            return result;
        }

        public override ApiResult<OrganizationDto> Update([FromBody] OrganizationDto item)
        {
            var result = base.Update(item);
            _uow.SaveChanges(true);
            return result;
        }

        public override ApiResult<string> Delete([FromBody] OrganizationDto item)
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
