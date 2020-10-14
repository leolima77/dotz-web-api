using Dotz.Common.Api;
using Dotz.Common.Api.Base;
using Dotz.Data.Entities;
using Dotz.Dto;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using AutoMapper;
using System.Linq;

namespace Dotz.WebApi.Controllers
{
    [ApiController]
    [Route("AppResource")]
    public class AppResourceController : ApiBase<AppResource, AppResourceDto, AppResourceController>
    {
        private readonly IMemoryCache _memoryCache;
        public AppResourceController(IServiceProvider service) : base(service)
        {
            _memoryCache = service.GetService<IMemoryCache>();
        }

        public override ApiResult<AppResourceDto> Add([FromBody] AppResourceDto item)
        {
            var result = base.Add(item);
            _uow.SaveChanges(false);
            return result;
        }

        public override ApiResult<AppResourceDto> Update([FromBody] AppResourceDto item)
        {
            var result = base.Update(item);
            _uow.SaveChanges(true);
            return result;
        }

        public override ApiResult<string> Delete([FromBody] AppResourceDto item)
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

        [HttpGet("GetResourcesByLanguage")]
        [ResponseCache(Duration = 120, Location = ResponseCacheLocation.Client)]
        public ApiResult<List<AppResourceDto>> GetResourcesByLanguage (Guid LanguageId)
        {
            if (!_memoryCache.TryGetValue(LanguageId, out List<AppResourceDto> ResourceList))
            {
                ResourceList = GetQueryable().Where(x=>x.LanguageId == LanguageId).ToList().Select(x => Mapper.Map<AppResourceDto>(x)).ToList();
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                            .SetPriority(CacheItemPriority.Normal)
                            .SetSlidingExpiration(TimeSpan.FromDays(1));
                _memoryCache.Set(LanguageId, ResourceList, cacheEntryOptions);
            }

            return new ApiResult<List<AppResourceDto>>
            {
                StatusCode = StatusCodes.Status200OK,
                Message = "Resource founded.",
                Body = ResourceList
            };
        }

        [HttpGet]
        public ApiResult ClearCache(Guid LanguageId)
        {
            _memoryCache.Remove(LanguageId);
            return new ApiResult
            {
                StatusCode = StatusCodes.Status200OK,
                Message = "Cache removed from the server.",
                Body = null
            };
        }
    }
}
