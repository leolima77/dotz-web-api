using AutoMapper;
using Dotz.Common.Paging;
using Dotz.Common.Paging.Interface;
using Dotz.Common.Repository;
using Dotz.Common.UnitofWork;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Dotz.Common.Api.Base
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ApiBase<T, TDto, TController> : ControllerBase where T : class where TDto : class where TController : ControllerBase
    {
        #region Variables

        public readonly IUnitofWork _uow;

        public readonly IServiceProvider _service;
        public readonly ILogger<TController> _logger;
        private readonly IGenericRepository<T> _repository;
        private IHttpContextAccessor _httpContextAccessor;

        #endregion Variables

        #region Constructor

        public ApiBase(IServiceProvider service)
        {
            _logger = service.GetService<ILogger<TController>>();
            _uow = service.GetService<IUnitofWork>();
            _repository = _uow.GetRepository<T>();
            _service = service;
            _httpContextAccessor = service.GetService<IHttpContextAccessor>();
        }

        #endregion Constructor

        #region GetMethods

        private Guid GetCurrentUser()
        {
            var userClaim = _httpContextAccessor.HttpContext.User.FindFirst("jti");
            Guid.TryParse((userClaim != null ? userClaim.Value : ""), out Guid userId);
            return userId != null ? userId : Guid.Empty;
        }

        [HttpGet("Find")]
        public virtual ApiResult<TDto> Find(Guid id)
        {
            try
            {
                _logger.LogInformation($"Find record from the {typeof(T)} table with id:{id}");

                return new ApiResult<TDto>
                {
                    Message = "Success",
                    StatusCode = StatusCodes.Status200OK,
                    Body = Mapper.Map<T, TDto>(_repository.Find(id))
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Find record error from the {typeof(T)} table with id:{id} error:{ex}");
                return new ApiResult<TDto>
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = $"Error:{ex.Message}",
                    Body = null
                };
            }
        }

        [HttpGet("FindAsync")]
        public virtual async Task<ApiResult<TDto>> FindAsync(Guid id)
        {
            return Find(id);
        }

        [HttpGet("GetAll")]
        public virtual ApiResult<List<TDto>> GetAll()
        {
            try
            {
                var entities = _repository.GetAll().ToList();
                _logger.LogInformation($"Getall records from the {typeof(T)} table. UserId:{GetCurrentUser()}");
                return new ApiResult<List<TDto>>
                {
                    Message = "Success",
                    StatusCode = StatusCodes.Status200OK,
                    Body = entities.Select(x => Mapper.Map<TDto>(x)).ToList()
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Getall records error from the {typeof(T)} table. UserId:{GetCurrentUser()}");
                return new ApiResult<List<TDto>>
                {
                    Message = $"Error:{ex.Message}",
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Body = null
                };
            }
        }

        [HttpGet("GetQueryable")]
        public virtual IQueryable<T> GetQueryable()
        {
            try
            {
                _logger.LogInformation($"GetQueryable from the {typeof(T)} table. UserId:{GetCurrentUser()}");
                return _repository.GetAll();
            }
            catch (Exception Ex)
            {
                _logger.LogError($"GetQueryable error from the {typeof(T)} table. UserId:{GetCurrentUser()} Error:{Ex}");
                return null;
            }
        }

        [HttpPost("GetAllWithPaging")]
        public virtual ApiResult GetAllWithPaging(PagingParams pagingParams)
        {
            try
            {
                _logger.LogInformation($"GetAllWithPaging from the {typeof(T)} table. UserId:{GetCurrentUser()}");
                var pagingLinks = _service.GetService<IPagingLinks<T>>();

                var model = new PagedList<T>(
                    GetQueryable(), pagingParams.PageNumber, pagingParams.PageSize);

                Response.Headers.Add("X-Pagination", model.GetHeader().ToJson());

                var outputModel = new OutputModel<T>
                {
                    Paging = model.GetHeader(),
                    Links = pagingLinks.GetLinks(model),
                    Items = model.List.Select(m => m).ToList(),
                };

                return new ApiResult
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Success",
                    Body = outputModel
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetAllWithPaging error from the {typeof(T)} table. UserId:{GetCurrentUser()} Data: {String.Join(',', pagingParams.GetType().GetProperties().Select(x => $" - {x.Name} : {x.GetValue(pagingParams)} - ").ToList())} exception:{ex}");
                return new ApiResult
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = $"Error:{ex.Message}",
                    Body = null
                };
            }
        }

        #endregion GetMethods

        #region PostMethods

        [HttpPost("DeleteById")]
        public virtual ApiResult<string> DeleteById(Guid id)
        {
            try
            {
                _repository.Delete(id);
                _logger.LogInformation($"Record deleted from the {typeof(T)} table. UserId:{GetCurrentUser()} with id:{id}");
                return new ApiResult<string>
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Success",
                    Body = null
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Delete error for the:{typeof(T)} table. UserId:{GetCurrentUser()} with id:{id} result:Error - {ex}");
                return new ApiResult<string>
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = $"Error:{ex.Message}",
                    Body = null
                };
            }
        }

        [HttpPost("DeleteByIdAsync")]
        public virtual async Task<ApiResult<string>> DeleteByIdAsync(Guid id)
        {
            return DeleteById(id);
        }

        [HttpPost("Delete")]
        public virtual ApiResult<string> Delete([FromBody] TDto item)
        {
            var resolvedItem = String.Join(',', item.GetType().GetProperties().Select(x => $" - {x.Name} : {x.GetValue(item)} - ").ToList());
            try
            {
                _repository.Delete(Mapper.Map<T>(item));
                _logger.LogInformation($"Record deleted from the {typeof(T)} table. UserId:{GetCurrentUser()} Data:{resolvedItem}");
                return new ApiResult<string>
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Success",
                    Body = null
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Delete record error from the {typeof(T)} table. UserId:{GetCurrentUser()} Data: {resolvedItem} exception:{ex}");
                return new ApiResult<string>
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = $"Error:{ex.Message}",
                    Body = null
                };
            }
        }

        [HttpPost("DeleteAsync")]
        public virtual async Task<ApiResult<string>> DeleteAsync([FromBody] TDto item)
        {
            return Delete(item);
        }

        [HttpPost("Add")]
        public virtual ApiResult<TDto> Add([FromBody] TDto item)
        {
            var resolvedResult = "";
            try
            {
                var TResult = _repository.Add(Mapper.Map<T>(item));
                resolvedResult = String.Join(',', TResult.GetType().GetProperties().Select(x => $" - {x.Name} : {x.GetValue(TResult) ?? ""} - ").ToList());
                _logger.LogInformation($"Add record to the {typeof(T)} table. UserId:{GetCurrentUser()} Data:{resolvedResult}");
                return new ApiResult<TDto>
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Success",
                    Body = Mapper.Map<T, TDto>(TResult)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Add record error to the {typeof(T)} table. UserId:{GetCurrentUser()} Data: {resolvedResult} exception:{ex}");
                return new ApiResult<TDto>
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = $"Error:{ex.Message}",
                    Body = null
                };
            }
        }

        [HttpPost("AddAsync")]
        public virtual async Task<ApiResult<TDto>> AddAsync([FromBody] TDto item)
        {
            return Add(item);
        }

        [HttpPost("Update")]
        public virtual ApiResult<TDto> Update([FromBody] TDto item)
        {
            var resolvedItem = String.Join(',', item.GetType().GetProperties().Select(x => $" - {x.Name} : {x.GetValue(item)} - ").ToList());
            try
            {
                var TResult = _repository.Update(Mapper.Map<T>(item));
                _logger.LogInformation($"Update record to the {typeof(T)} table. UserId:{GetCurrentUser()} Data:{resolvedItem}");
                return new ApiResult<TDto>
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Success",
                    Body = Mapper.Map<T, TDto>(TResult)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Update record error to the {typeof(T)} table. UserId:{GetCurrentUser()} Data: {resolvedItem} exception:{ex}");
                return new ApiResult<TDto>
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = $"Error:{ex.Message}",
                    Body = null
                };
            }
        }

        [HttpPost("UpdateAsync")]
        public virtual async Task<ApiResult<TDto>> UpdateAsync([FromBody] TDto item)
        {
            return Update(item);
        }

        #endregion PostMethods

        #region SaveChanges

        private void Save()
        {
            _uow.SaveChanges(true);
        }

        #endregion SaveChanges
    }
}