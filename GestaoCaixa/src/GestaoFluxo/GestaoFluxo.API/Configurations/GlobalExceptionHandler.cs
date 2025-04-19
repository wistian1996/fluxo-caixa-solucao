using GestaoFluxo.Application.PipelineBehavior;
using MediatR;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace GestaoFluxo.API.Configurations
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            var problemDetails = new ProblemDetails
            {
                Title = "Ocorreu um erro interno, tente novamente ou entre em contato com o suporte.",
                Status = StatusCodes.Status500InternalServerError,
                Detail = exception.Message,
                Instance = httpContext.Request.Path
            };

            if (exception is FluentValidation.ValidationException fluentException)
            {
                problemDetails.Title = "Ocorreram erros de validação.";
                problemDetails.Status = StatusCodes.Status400BadRequest;
                problemDetails.Detail = null;

                var validationErrors = new List<string>();
                foreach (var error in fluentException.Errors)
                {
                    validationErrors.Add(error.ErrorMessage);
                }
                problemDetails.Extensions.Add("errors", validationErrors);

                // _logger.LogWarning("{ProblemDetailsTitle}", problemDetails.Title);
            }

            httpContext.Response.StatusCode = problemDetails.Status.Value;

            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

            return true;
        }
    }
}
