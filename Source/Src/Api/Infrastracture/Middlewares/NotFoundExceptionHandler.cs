using Microsoft.AspNetCore.Diagnostics;

namespace CleanArchitecture.WebApi.Infrastracture.Middlewares
    {
    public sealed class NotFoundExceptionHandler : IExceptionHandler
        {
        private readonly ILogger<NotFoundExceptionHandler> _logger;

        public NotFoundExceptionHandler(ILogger<NotFoundExceptionHandler> logger)
            {
            _logger = logger;
            }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
            {
            if (exception is not FileNotFoundException notFoundException)//or any other DirectoryNotFoundException
                {
                return false;
                }

            _logger.LogError(
                notFoundException,
                "Exception occurred: {Message}",
                notFoundException.Message);

            var problemDetails = new ProblemDetails
                {
                Status = StatusCodes.Status404NotFound,
                Title = "Not Found",
                Detail = notFoundException.Message
                };

            httpContext.Response.StatusCode = problemDetails.Status.Value;

            await httpContext.Response
                .WriteAsJsonAsync(problemDetails, cancellationToken);

            return true;
            }
        }
    }
