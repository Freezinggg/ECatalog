using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECatalog.Application.Common
{
    public class LoggingBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

        public LoggingBehavior(
            ILogger<LoggingBehavior<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            var requestName = typeof(TRequest).Name;
            try
            {
                using (_logger.BeginScope(CreateScope(requestName, request)))
                {
                    _logger.LogInformation("Handling {RequestName}", requestName);
                    var response = await next();
                    if (response is not null) TryLogResult(requestName, response);

                    return response;
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Unhandled exception while handling {RequestName}",
                    requestName);

                throw;
            }
            
        }

        private void TryLogResult(string requestName, TResponse response)
        {
            try
            {
                dynamic result = response;

                if (result.IsSuccess)
                {
                    _logger.LogInformation(
                        "Handled {RequestName} successfully",
                        requestName);
                }
                else
                {
                    _logger.LogWarning(
                    "Handled {RequestName} with status {Status}. Error: {Error}",
                    new object[]
                    {
                        requestName,
                        result.Status,
                        result.ErrorMessage
                    });
                }
            }
            catch
            {
                
            }
        }

        private static IDictionary<string, object> CreateScope(string requestName, TRequest request)
        {
            var scope = new Dictionary<string, object>
            {
                ["RequestName"] = requestName
            };

            // If command has Id, include it
            var idProp = request?.GetType().GetProperty("Id");
            if (idProp != null)
            {
                var idValue = idProp.GetValue(request);
                scope["EntityId"] = idValue!;
            }

            return scope;
        }
    }

}
