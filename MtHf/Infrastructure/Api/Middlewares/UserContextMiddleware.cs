using Infrastructure.Common;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace OutboxHf.Middlewares;

public class UserContextMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        string userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
        UserContext.CurrentUserId = userId;

        await _next(context);
    }
}
