using System.Net;
using FuturisX.AspNetCore.Mvc.Extensions;
using FuturisX.AspNetCore.Mvc.Models;
using FuturisX.AspNetCore.Mvc.Results;
using FuturisX.Dependency;
using FuturisX.Entities;
using FuturisX.Security;
using FuturisX.Validation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace FuturisX.AspNetCore.Mvc.ExceptionHandling
{
    public class ExceptionFilter : IExceptionFilter, ITransientDependency
    {
        private readonly IErrorInfoBuilder _errorInfoBuilder;
        private readonly ILogger _logger;

        public ExceptionFilter(IErrorInfoBuilder errorInfoBuilder, ILogger<ExceptionFilter> logger)
        {
            _errorInfoBuilder = errorInfoBuilder;
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            if (!context.ActionDescriptor.IsControllerAction())
                return;

            _logger.LogError(context.Exception, null, null);

            HandleAndWrapException(context);
        }

        private void HandleAndWrapException(ExceptionContext context)
        {
            if (!ActionResultHelper.IsObjectResult(context.ActionDescriptor.GetMethodInfo().ReturnType))
                return;

            context.HttpContext.Response.StatusCode = GetStatusCode(context);

            context.Result = new ObjectResult(
                    new AjaxResponse(
                            _errorInfoBuilder.BuildForException(context.Exception),
                            context.Exception is SecurityException
                        )
                );

            context.Exception = null; // Handled!
        }

        private int GetStatusCode(ExceptionContext context)
        {
            if (context.Exception is SecurityException)
            {
                return context.HttpContext.User.Identity.IsAuthenticated
                    ? (int)HttpStatusCode.Forbidden
                    : (int)HttpStatusCode.Unauthorized;
            }

            if (context.Exception is ValidationException)
            {
                return (int)HttpStatusCode.BadRequest;
            }

            if (context.Exception is EntityNotFoundException)
            {
                return (int)HttpStatusCode.NotFound;
            }

            return (int)HttpStatusCode.InternalServerError;
        }
    }
}