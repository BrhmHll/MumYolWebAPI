using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Core.Utilities.Exceptions;
using Core.Utilities.Results;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;

namespace Core.Extensions
{
    public class ExceptionMiddleware
    {
        private RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception e)
            {
                await HandleExceptionAsync(httpContext,e);
            }
        }

        private Task HandleExceptionAsync(HttpContext httpContext, Exception e)
        {
            httpContext.Response.ContentType = "application/json";
            
            int statusCode = (int) HttpStatusCode.InternalServerError;
            string message = e.Message;
            //string message = "Internal Server Error";

            IEnumerable<ValidationFailure> errors;
            if (e.GetType() == typeof(ValidationException))
            {
                message = e.Message;
                errors = ((ValidationException)e).Errors;
                statusCode = (int)HttpStatusCode.Forbidden;
                //return httpContext.Response.WriteAsync(new ValidationErrorDetails
                //{
                //    Message = "Hatalı Veri",
                //    StatusCode = statusCode,
                //    Errors = errors

                //}.ToString());

            }
            else if (e.GetType() == typeof(AuthorizationDeniedException))
            {
                statusCode = (int) HttpStatusCode.Unauthorized;
            }
            httpContext.Response.StatusCode = statusCode;

            return httpContext.Response.WriteAsync(new ErrorResult(message).ToString());
        }
    }
}
