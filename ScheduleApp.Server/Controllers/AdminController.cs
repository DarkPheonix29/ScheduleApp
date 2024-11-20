using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/admin")]
public class AdminController : ControllerBase
{
	private readonly FirebaseRoles _authService;
	private readonly FirebaseKey _keyService;

	public AdminController(FirebaseRoles authService, FirebaseKey keyService)
	{
		_authService = authService;
		_keyService = keyService;
	}

	// POST: api/admin/assign-role
	[HttpPost("assign-role")]
	public async Task<IActionResult> AssignRole([FromBody] AssignRoleRequest request)
	{
		await _authService.AssignRoleAsync(request.UserId, request.Role);
		return Ok(new { message = "Role assigned successfully." });
	}

	// POST: api/admin/generate-key
	[HttpPost("generate-key")]
	public async Task<IActionResult> GenerateKey()
	{
		var key = await _keyService.GenerateRegistrationKeyAsync();
		return Ok(new { key });
	}

	// POST: api/admin/use-key
	[HttpPost("use-key")]
	public async Task<IActionResult> UseKey([FromBody] string key)
	{
		var success = await _keyService.UseRegistrationKeyAsync(key);
		return success ? Ok(new { message = "Key used successfully." }) : BadRequest(new { message = "Invalid or already used key." });
	}
}

public class AssignRoleRequest
{
	public string UserId { get; set; }
	public string Role { get; set; }
}