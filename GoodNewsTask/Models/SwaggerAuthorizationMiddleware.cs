﻿using Microsoft.AspNetCore.Authorization;

namespace GoodNewsTask.Models
{
    public class SwaggerAuthorizationMiddleware //Ограничение доступа к Swagger через middleware https://zzzcode.ai/answer-question?id=39f62f08-90a4-4dc0-952a-6196a8c1a713
    {
        private readonly RequestDelegate _next;

        public SwaggerAuthorizationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            // Проверяем, что запрос идет к Swagger и пользователь не аутентифицирован
            if (context.Request.Path.StartsWithSegments("/swagger"))
            {
                // Проверяем, аутентифицирован ли пользователь и имеет ли он роль "Admin"
                if (!context.User.Identity.IsAuthenticated || !context.User.IsInRole("Admin"))
                {
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    await context.Response.WriteAsync("This page is opened only for admin");
                    return;
                }
            }

            await _next(context);
        }
    }
}
