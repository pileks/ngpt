using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Ngpt.Platform.Identity.Web.Services;

namespace Ngpt.Platform.Identity.Web.Middleware.TokenAuthentication
{
    public class TokenAuthenticationMiddleware
    {
        private readonly RequestDelegate next;

        public TokenAuthenticationMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context, AuthTokenService authTokenService)
        {
           await this.SetAuthToken(context, authTokenService);

           await this.next(context);
        }

        private async Task SetAuthToken(HttpContext context, AuthTokenService authTokenService)
        {
            var token = context.Request.Headers["Authorization"].ToString();
            authTokenService.RequestToken = token;

            await authTokenService.ExtendAuthTokenExpirationIfExists();
        }
    }
}