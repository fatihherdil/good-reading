using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using GoodReading.Application.ResponseModels;
using GoodReading.Domain.Exceptions;
using Microsoft.AspNetCore.Http;

namespace GoodReading.Web.Api.Middlewares
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;
        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            const HttpStatusCode code = HttpStatusCode.InternalServerError;

            var responseCode = ex is IHttpException exception ? (int) exception.StatusCode : (int) code;
            
            var result = JsonSerializer.Serialize(new DefaultResponse((HttpStatusCode) responseCode, ex.Message));
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = responseCode;
            return context.Response.WriteAsync(result);
        }
    }
}
