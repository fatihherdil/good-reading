using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using GoodReading.Application.ResponseModels;
using GoodReading.Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Serilog;

namespace GoodReading.Web.Api.Middlewares
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger _logger;
        public ErrorHandlingMiddleware(RequestDelegate next, ILogger logger)
        {
            this.next = next;
            this._logger = logger;
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

        private Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            _logger.Error(ex, "An Unknown Error Occured !");
            const HttpStatusCode code = HttpStatusCode.InternalServerError;

            var responseCode = ex is IHttpException exception ? (int) exception.StatusCode : (int) code;
            
            var result = JsonSerializer.Serialize(new DefaultResponse((HttpStatusCode) responseCode, ex.Message));
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = responseCode;
            return context.Response.WriteAsync(result);
        }
    }
}
