using Microsoft.AspNetCore.Http;
using System.Security.Claims;

public class RoleAuth
{
	private readonly RequestDelegate _next;

	public RoleAuth(RequestDelegate next)
	{
		_next = next;
	}

	public async Task Invoke(HttpContext context)
	{
		if (context.User.Identity?.IsAuthenticated == true)
		{
			// Extract role claim
			var roleClaim = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);
			if (roleClaim == null || string.IsNullOrWhiteSpace(roleClaim.Value))
			{
				context.Response.StatusCode = StatusCodes.Status403Forbidden;
				await context.Response.WriteAsync("Access denied. Role is missing.");
				return;
			}
		}

		// Proceed to the next middleware
		await _next(context);
	}
}
