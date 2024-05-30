using PublicCommon;

namespace CleanArchitecture.WebApi.Infrastracture.Middlewares
    {
    // Middleware to decrypt and validate the request
    public class EncryptionValidationMiddleware(RequestDelegate next)
        {
        public async Task Invoke(HttpContext context)
            {
            // Currently only api validation handling //this applies only for aspnet core hosted blazor, for normal api only controller so wont apply
            //for others only CORS yet to handle //todo
            if (!context.Request.Path.StartsWithSegments("/api"))
                {
                await next(context);
                return;
                }


            // Check if the custom header exists
            if (context.Request.Headers.TryGetValue(CONSTANTS.AppKeyName, out var appKey))
                {
                // Console.WriteLine(appKey);
                // Decrypt the content
                //string decryptedContent = Encryption.DecryptString("your-shared-key", appKey);
                //encryption is having some issue ,so running durect test
                string decryptedContent = appKey;
                // Validate the decrypted content
                if (IsValid(appKey))
                    {
                    // Allow the request to proceed
                    await next(context);
                    }
                else
                    {
                    // Respond with an error if validation fails
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Invalid request content(Key)");
                    }
                }
            else
                {
                Console.WriteLine("no key passed");
                // If the header is not present, proceed with the request
                // This allows for anonymous access points
                return;//since no key,so dont repond
                //await next(context);
                }
            }

        private bool IsValid(string content)
            {
            //Console.WriteLine("key is " + content ?? "blank");
            if (content == PublicCommon.CONSTANTS.MyTownAppKey || content == PublicCommon.CONSTANTS.MyTownAppKeyAuth)
                return true; // Placeholder for actual validation
            Console.WriteLine("key is " + content ?? "blank");
            return false;
            }
        }

    // Extension method to add the middleware
    public static class EncryptionValidationMiddlewareExtensions
        {
        public static IApplicationBuilder UseEncryptionValidation(this IApplicationBuilder builder)
            {
            return builder.UseMiddleware<EncryptionValidationMiddleware>();
            }
        }

    }
