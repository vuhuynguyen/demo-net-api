using ApplicationCore.Common.Seedwork.Authorization;
using ApplicationCore.Domain.Shared.Exceptions;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace WebApi.Infrastructure
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<GlobalExceptionFilter> _logger;
        private readonly IMediator _mediator;

        public GlobalExceptionFilter(IWebHostEnvironment env, ILogger<GlobalExceptionFilter> logger, IMediator mediator)
        {
            _env = env;
            _logger = logger;
            _mediator = mediator;
        }

        public void OnException(ExceptionContext context)
        {
            var ex = context.Exception.GetOriginalException();
            var exMessage = ex.Message;
            _logger.LogError(ex, exMessage);

            ProblemDetails problem;
            if (context.Exception.GetType() == typeof(NotFoundException))
            {
                problem = new ProblemDetails
                {
                    Status = (int)HttpStatusCode.NotFound,
                    Title = exMessage
                };
            }
            else if (context.Exception.GetType().IsAssignableTo(typeof(UnauthorizedException)))
            {
                problem = new ProblemDetails
                {
                    Status = (int)HttpStatusCode.Forbidden,
                    Title = exMessage
                };
            }
            else if (context.Exception.GetType() == typeof(ValidationException))
            {
                problem = new ProblemDetails
                {
                    Status = (int)HttpStatusCode.BadRequest,
                    Title = exMessage
                };
            }
            else if (context.Exception.GetType().IsAssignableTo(typeof(BusinessException)))
            {
                problem = new ProblemDetails
                {
                    Status = (int)HttpStatusCode.OK,
                    Title = exMessage
                };

                problem.Extensions.Add("errorCode", "BUSINESS_ERROR");
            }
            else if (context.Exception.GetType().IsAssignableTo(typeof(DbConcurrencyException)))
            {
                problem = new ProblemDetails
                {
                    Status = (int)HttpStatusCode.OK,
                };
                var exception = (DbConcurrencyException)context.Exception;
                var fullName = string.Empty;

                if (!string.IsNullOrEmpty(exception.ModificationBy))
                {
                    fullName = "Test User";
                }

                problem.Extensions.Add("errorCode", "DB_CONCURRENCY_UPDATE");
                problem.Extensions.Add("ModificationBy", fullName);
            }
            else
            {
                problem = new ProblemDetails
                {
                    Status = (int)HttpStatusCode.InternalServerError,
                    Title = "An error occurred. Please try again.",
                };

                if (_env.EnvironmentName.ToUpper() == "DEVELOPMENT")
                {
                    problem.Detail = ex.ToString();
                }
            }

            context.Result = new ObjectResult(problem);
            context.ExceptionHandled = true;
        }
    }
}
