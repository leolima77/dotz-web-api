using Dotz.Common.Api;
using Dotz.Common.Api.Base;
using Dotz.Data.Entities;
using Dotz.Dto;
using Dotz.WebApi.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace Dotz.WebApi.Controllers
{
    [ApiController]
    [Route("Language")]
    public class LanguageController : ApiBase<Language, LanguageDto, LanguageController>, ILanguageController
    {
        #region Constructor

        public LanguageController(IServiceProvider service) : base(service)
        {
        }

        #endregion Constructor

        [AllowAnonymous]
        public override ApiResult<LanguageDto> Add([FromBody] LanguageDto item)
        {
            if (!CheckDublicateLanguage(item.Culture))
            {
                var result = base.Add(item);
                _uow.SaveChanges(false);
                return result;
            }
            else
            {
                return new ApiResult<LanguageDto>
                {
                    StatusCode = StatusCodes.Status406NotAcceptable,
                    Message = "This culture is exist!",
                    Body = null
                };
            }
        }

        [HttpGet("CheckDublicateLanguage")]
        public bool CheckDublicateLanguage(string culture)
        {
            var result = GetQueryable().Where(x => x.Culture == culture).ToList().Count;
            return result > 0;
        }

        public override ApiResult<LanguageDto> Update([FromBody] LanguageDto item)
        {
            var result = base.Update(item);
            _uow.SaveChanges(true);
            return result;
        }

        public override ApiResult<string> Delete([FromBody] LanguageDto item)
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