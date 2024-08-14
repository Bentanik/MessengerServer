namespace MessengerServer.Src.WebApi.Middlewares;

public class AccessTokenValidationMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        var token = context.Request.Headers["IS-TOKEN-EXPIRED"];
        context.Response.Headers.Add("X-Custom-Header", "CustomHeaderValue");

        await _next(context);
    }
}
