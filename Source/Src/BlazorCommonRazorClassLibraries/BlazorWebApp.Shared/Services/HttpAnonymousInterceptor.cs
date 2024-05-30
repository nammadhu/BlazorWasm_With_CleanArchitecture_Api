namespace BlazorWebApp.Shared.Services
    {

    public class HttpAnonymousInterceptor : DelegatingHandler
        {

        //this gets intercepted in calling Anonymous endpoints 
        //must be httpclient of name "AnonymousClient" then it gets auto intercepted
        public HttpAnonymousInterceptor()
            {
            }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
            // Encrypt the content
            //string encryptedContent = Encryption.EncryptString("your-shared-key", "madhu here kanri");
            if (!request.Headers.Contains(AnonymousHeader.Key))
                {
                request.Headers.Add(AnonymousHeader.Key, AnonymousHeader.Value);
                }

            return await base.SendAsync(request, cancellationToken);

            }
        }

    }
