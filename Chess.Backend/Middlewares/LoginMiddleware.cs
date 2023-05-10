using System.Security.Claims;
using Chess.Backend.Services;

namespace Chess.Backend.Middlewares;

public class LoginMiddleware
{
    private readonly RequestDelegate _next;
    private readonly SessionService _sessionService;

    public LoginMiddleware(RequestDelegate next, SessionService sessionService)
    {
        _next = next;
        _sessionService = sessionService;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var token = context.Request.Headers["chess-token"];
        if(Guid.TryParse(token, out var guid)){
            var userId =  _sessionService.GetUserId(guid);
            if(userId != null){
                context.Items["user-id"] =  userId.ToString();
            }
        }
        await _next(context);
    }
}