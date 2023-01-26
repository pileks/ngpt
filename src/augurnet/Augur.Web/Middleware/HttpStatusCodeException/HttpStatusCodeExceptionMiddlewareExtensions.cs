using Microsoft.AspNetCore.Builder;

namespace Augur.Web.Middleware.HttpStatusCodeException
{
    public static class HttpStatusCodeExceptionMiddlewareExtensions
    {
        public static IApplicationBuilder UseHttpStatusCodeExceptionMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AugurHttpStatusCodeExceptionMiddleware>();
        }
    }
}