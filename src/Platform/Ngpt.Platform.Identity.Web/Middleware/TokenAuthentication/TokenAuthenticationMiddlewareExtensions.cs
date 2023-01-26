using Microsoft.AspNetCore.Builder;

namespace Ngpt.Platform.Identity.Web.Middleware.TokenAuthentication
{
    public static class TokenAuthenticationMiddlewareExtensions
    {
        public static IApplicationBuilder UseTokenAuthenticationMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<TokenAuthenticationMiddleware>();
        }
    }
}