using Dotz.Common.Api;
using Dotz.Common.Api.Base;
using Dotz.Data.Entities;
using Dotz.Dto;
using Dotz.WebApi.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace Dotz.WebApi.Controllers
{
    [ApiController]
    [Route("Product")]
    public class ProductController : ApiBase<Product, ProductDto, ProductController>, IProductController
    {
        #region Constructor

        public ProductController(IServiceProvider service) : base(service)
        {
        }

        #endregion Constructor

        public override ApiResult<ProductDto> Add([FromBody] ProductDto item)
        {
            var result = base.Add(item);
            _uow.SaveChanges(false);
            return result;
        }

        public override ApiResult<ProductDto> Update([FromBody] ProductDto item)
        {
            var result = base.Update(item);
            _uow.SaveChanges(true);
            return result;
        }

        public override ApiResult<string> Delete([FromBody] ProductDto item)
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