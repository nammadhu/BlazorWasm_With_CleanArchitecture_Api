using FluentValidation;
using PublicCommon;
using System.Net;
using System.Text.Json;

namespace CleanArchitecture.WebApi.Infrastracture.Middlewares
    {
    //this is also valid but using .net 8 advanced feature of .AddExceptionHandler<GlobalExceptionHandler>(); IExceptionHandler

    public class ErrorHandlerMiddleware(RequestDelegate next)
        {
        public async Task Invoke(HttpContext context)
            {
            try
                {
                await next(context);
                }
            catch (Exception error)
                {
                var response = context.Response;
                response.ContentType = CONSTANTS.ApplicationJson;// "application/json";
                var responseModel = new BaseResult<string>(new Error(ErrorCode.Exception, error?.Message));

                switch (error)
                    {
                    case ValidationException e:
                        // validation error
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        responseModel.Errors = e.Errors.Select(p => new Error(ErrorCode.ModelStateNotValid, p.ErrorMessage, p.PropertyName)).ToList();
                        break;
                    case KeyNotFoundException e:
                        // not found error
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        break;
                    default:
                        // unhandled error
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                    }
                //var result = JsonSerializer.Serialize(responseModel, new JsonSerializerOptions()
                //{
                //    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                //});
                var result = JsonSerializer.Serialize(responseModel);
                //var result = JsonSerializer.Serialize(responseModel, new JsonSerializerOptions()
                //    {
                //    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                //});


                await response.WriteAsync(result);
                }
            }
        }
    }
