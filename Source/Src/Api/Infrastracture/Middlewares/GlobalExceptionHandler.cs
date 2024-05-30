﻿using Microsoft.AspNetCore.Diagnostics;

namespace CleanArchitecture.WebApi.Infrastracture.Middlewares
    {
    //(ExceptionHandlingMiddleware) is old way,GlobalExceptionHandler : IExceptionHandler is new way from asapnet core 8
    public sealed class GlobalExceptionHandler : IExceptionHandler
        {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
            {
            _logger = logger;
            }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
            {
            _logger.LogError(
                exception, "Exception occurrrrrred: {Message}", exception.Message);

            var problemDetails = new ProblemDetails
                {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Server error"
                };

            httpContext.Response.StatusCode = problemDetails.Status.Value;

            await httpContext.Response
                .WriteAsJsonAsync(problemDetails, cancellationToken);

            return true;
            }
        }
    }
