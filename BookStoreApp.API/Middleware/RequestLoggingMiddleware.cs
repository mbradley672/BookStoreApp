namespace BookStoreApp.API.Middleware
{
    internal class RequestLoggingMiddleware(ILogger<RequestLoggingMiddleware> logger) : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            // Code logic here
            try
            {
                await next(context);
            }
            catch (Exception e)
            {
                logger.LogInformation($"Request made " +
                                      $"METHOD:{context.Request.Method}, " +
                                      $"URL: {context.Request.Path.Value} " +
                                      $"=> {context.Response.StatusCode}");
                throw;
            }
        }
    }
}
