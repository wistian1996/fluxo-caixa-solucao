using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GestaoFluxo.Application.PipelineBehavior
{
    public sealed class LoggingBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse> where TRequest : class
    {
        private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

        public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            Stopwatch stopwatch = new();

            _logger.LogInformation("Handling command {CommandName} ({@Command})", request.GetType(), request);

            var response = await next();

            stopwatch.Stop();

            _logger.LogInformation("Command {CommandName} handled in {stopwatch.ElapsedMilliseconds} ms " +
                "- response: {@Response}", request.GetType(), stopwatch.ElapsedMilliseconds,response);

            return response;
        }
    }
}
