using Microsoft.AspNetCore.Builder;
using Scanner.Helper.Middlewares;
using Scanner.Helper.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scanner.Helper.Extensions
{
    public static class ApiResponseMiddlewareExtension
    {
        public static IApplicationBuilder UseApiResponseAndExceptionWrapper(this IApplicationBuilder builder, ApiResponseOptions options = default)
        {
            options ??= new ApiResponseOptions();
            return builder.UseMiddleware<ApiResponseMiddleware>(options);
        }
    }
}
