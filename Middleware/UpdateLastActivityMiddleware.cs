using System.Security.Claims;
using eproject.Data;

public class UpdateLastActivityMiddleware
{
    private readonly RequestDelegate _next;

    public UpdateLastActivityMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, AppDbContext dbContext)
    {
        if (context.User.Identity.IsAuthenticated)
        {
            var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await dbContext.Users.FindAsync(userId);
            if (user != null)
            {
                user.LastActivityDate = DateTime.Now;
                await dbContext.SaveChangesAsync();
            }
        }

        await _next(context);
    }
}