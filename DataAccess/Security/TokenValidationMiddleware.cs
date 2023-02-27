using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Security
{
    public class TokenValidationMiddleware : IMiddleware
    {
        private readonly ITokenBlacklist _tokenBlacklist;

        public TokenValidationMiddleware(ITokenBlacklist tokenBlacklist)
        {
            _tokenBlacklist = tokenBlacklist;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            string token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (!string.IsNullOrEmpty(token) && _tokenBlacklist.IsTokenBlacklisted(token))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Unauthorized");
                return;
            }

            await next(context);
        }
    }
    }
