using BLL.Firebase;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[ApiController]
[Route("api/admin")]
public class AdminController : ControllerBase
{
	private readonly FirebaseRoles _authService;
	private readonly FirebaseKey _KeyService;
	private readonly FirebaseKeyManager _firebaseKeyManager;

	public AdminController(FirebaseRoles authService, FirebaseKey keyService)
	{
		_authService = authService;
		_KeyService = keyService;
	}

	// POST: api/admin/assign-role
	[HttpPost("assign-role")]
	public async Task<IActionResult> AssignRole([FromBody] AssignRoleRequest request)
	{
		// Assign a role to a user
		await _authService.AssignRoleAsync(request.UserId, request.Role);
		return Ok(new { message = "Role assigned successfully." });
	}

	// POST: api/admin/generate-key
	[HttpPost("generate-key")]
	public async Task<IActionResult> GenerateKey()
	{
		// Generate a new registration key
		var key = await _KeyService.GenerateRegistrationKeyAsync();
		return Ok(new { key });
	}

	// POST: api/admin/use-key
	[HttpPost("use-key")]
	public async Task<IActionResult> UseKey([FromBody] string key)
	{
		// Mark a registration key as used
		var success = await _KeyService.UseRegistrationKeyAsync(key);
		return success ? Ok(new { message = "Key used successfully." }) : BadRequest(new { message = "Invalid or already used key." });
	}

	// GET: api/admin/keys
	[HttpGet("keys")]
	public async Task<IActionResult> GetKeys()
	{
		// Retrieve all available keys
		var keys = await _KeyService.GetAllKeysAsync();
		if (keys == null || keys.Count == 0)
		{
			return NotFound(new { message = "No keys found." });
		}
		return Ok(keys);
	}
}

public class AssignRoleRequest
{
	public string UserId { get; set; }
	public string Role { get; set; }
}