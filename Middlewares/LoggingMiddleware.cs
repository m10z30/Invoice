namespace Invoice.Middlewares
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LoggingMiddleware> _logger;

        public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            await _next(context);
            
            var timestamp = DateTime.UtcNow.ToString();
            var method = context.Request.Method;
            var path = context.Request.Path;
            var statusCode = context.Response.StatusCode;
            var contentLength = context.Response.ContentLength;

            _logger.LogInformation(
                "[{timestamp}] {method} {path} {statusCode} {contentLength}", 
                timestamp,
                method,
                path,
                statusCode,
                contentLength    
            );
        

        }
    }
}