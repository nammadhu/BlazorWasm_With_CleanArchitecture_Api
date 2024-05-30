namespace BlazorWebAppAuto.Middleware
    {
    //(ExceptionHandlingMiddleware) is old way,GlobalExceptionHandler : IExceptionHandler is new way from asapnet core 8
    public class ExceptionHandlingMiddleware
        { //not using now
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(
            RequestDelegate next,
            ILogger<ExceptionHandlingMiddleware> logger)
            {
            _next = next;
            _logger = logger;
            }

        public async Task InvokeAsync(HttpContext context)
            {
            try
                {
                await _next(context);
                }
            catch (Exception exception)
                {
                _logger.LogError(
                    exception, "Exception occurred: {Message}", exception.Message);

                var problemDetails = new
                    {
                    Status = StatusCodes.Status500InternalServerError,
                    Title = "Server Error"
                    };

                context.Response.StatusCode =
                    StatusCodes.Status500InternalServerError;

                await context.Response.WriteAsJsonAsync(problemDetails);
                }
            }
        }
    }
