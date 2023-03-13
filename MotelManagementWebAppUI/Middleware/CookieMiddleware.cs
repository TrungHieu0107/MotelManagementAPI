using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace MotelManagementWebAppUI.Middleware
{
    public class CookieMiddleware
    {
        private readonly RequestDelegate _next;

        public CookieMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Response.StatusCode == 401)
            {

                // Redirect to login page
                context.Response.Redirect("/Account/Login");
            }
            if (context.Response.StatusCode == 403)
            {

                // Redirect to login page
                context.Response.Redirect("/Account/Login");
            }

            await _next(context);
        }
    }
}
