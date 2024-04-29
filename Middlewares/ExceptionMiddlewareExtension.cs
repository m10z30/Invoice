using Invoice.Exceptions;
using Microsoft.AspNetCore.Diagnostics;

namespace Invoice.Middlewares
{
    public static class ExceptionMiddlewareExtension
    {
        public static void ConfigureExceptionMiddleware(this WebApplication app)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    context.Response.ContentType = "application/json";
                    var feature = context.Features.Get<IExceptionHandlerFeature>();
                    if (feature != null)
                    {
                        context.Response.StatusCode = feature.Error switch
                        {
                            _ => StatusCodes.Status500InternalServerError
                        };
                        var response = new ErrorResponse(feature.Error.Message);
                        if (context.Response.StatusCode == StatusCodes.Status500InternalServerError)
                        {
                            response.Error = "error, please retry again";
                        }
                        await context.Response.WriteAsync(response.ToString());
                    }
                });
            });
        }
    }
}