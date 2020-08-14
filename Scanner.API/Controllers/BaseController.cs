using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Scanner.Core.Abstract;
using Scanner.Helper.Messages;
using Scanner.Helper.Response.Models;

namespace Scanner.API.Controllers
{
    [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("scanapi/[controller]")]
    public class BaseController<T> : ControllerBase where T : BaseEntity
    {
        public IService<T> _storage;

        public BaseController(IService<T> storage)
        {
            _storage = storage;
        }

        [HttpPost]
        [Route("save")]
        public virtual async Task<ApiResponse> Save([FromBody] T value)
        {
            var newValue = await _storage.AddAsync(value);
            return new ApiResponse(ApiResponseMessage.Success, statusCode: (int)HttpStatusCode.OK, result: newValue);
        }

        [HttpPost]
        [Route("saverange")]
        public virtual async Task<ApiResponse> SaveRange([FromBody] IEnumerable<T> values)
        {
            var newValues = await _storage.AddRangeAsync(values);
            return new ApiResponse(ApiResponseMessage.Success, statusCode: (int)HttpStatusCode.OK, result: newValues);
        }

        [HttpDelete]
        [Route("hardremove/{id}")]
        public virtual async Task<ApiResponse> HardRemove(int id)
        {
            var value = await _storage.GetByIdAsync(id);
            _storage.Remove(value);
            return new ApiResponse(ApiResponseMessage.Success, statusCode: (int)HttpStatusCode.OK, result: true);
        }

        [HttpDelete]
        [Route("hardremoverange")]
        public virtual ApiResponse HardRemoveRange([FromBody] IEnumerable<T> values)
        {
            _storage.RemoveRange(values);
            return new ApiResponse(ApiResponseMessage.Success, statusCode: (int)HttpStatusCode.OK);
        }

        [HttpDelete]
        [Route("remove/{id}")]
        public virtual async Task<ApiResponse> Remove(int id)
        {
            var value = await _storage.GetByIdAsync(id);
            value.IsDeleted = true;
            _storage.Update(value);
            return new ApiResponse(ApiResponseMessage.Success, statusCode: (int)HttpStatusCode.OK, result: true);

        }

        [HttpPut]
        [Route("update")]
        public virtual ApiResponse Update([FromBody] T value)
        {
            var updatedValue = _storage.Update(value);
            return new ApiResponse(ApiResponseMessage.Success, statusCode: (int)HttpStatusCode.OK, result: updatedValue);

        }

        [HttpGet]
        [Route("getallasync")]
        public virtual async Task<ApiResponse> GetAllAsync()
        {
            var result = await _storage.GetAllAsync();
            return new ApiResponse(ApiResponseMessage.Success, statusCode: (int)HttpStatusCode.OK, result: result);

        }

        [HttpGet]
        [Route("getallasynconlystatus")]
        public virtual async Task<ApiResponse> GetAllAsyncOnlyStatus()
        {
            var result = await _storage.GetAllAsyncOnlyStatus();
            return new ApiResponse(ApiResponseMessage.Success, statusCode: (int)HttpStatusCode.OK, result: result);

        }

        [HttpGet]
        [Route("getbyidasync/{id}")]
        public virtual async Task<ApiResponse> GetByIdAsync(int id)
        {
            var result = await _storage.GetByIdAsync(id);
            return new ApiResponse(ApiResponseMessage.Success, statusCode: (int)HttpStatusCode.OK, result: result);

        }

        [HttpGet]
        [Route("whereasync")]
        public virtual async Task<ApiResponse> WhereAsync([FromBody] Expression<Func<T, bool>> predicate)
        {
            var result =  await _storage.Where(predicate);
            return new ApiResponse(ApiResponseMessage.Success, statusCode: (int)HttpStatusCode.OK, result: result);
        }

        [HttpGet]
        [Route("singledefaultasync")]
        public virtual async Task<ApiResponse> SingleOrDefaultAsync([FromBody] Expression<Func<T, bool>> predicate)
        {
            var result = await _storage.SingleOrDefaultAsync(predicate);
            return new ApiResponse(ApiResponseMessage.Success, statusCode: (int)HttpStatusCode.OK, result: result);

        }


    }
}
