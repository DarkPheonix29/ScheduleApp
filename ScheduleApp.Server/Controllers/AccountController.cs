using BLL.Interfaces;
using BLL.Manager;
using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ScheduleApp.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AccountController : ControllerBase
	{
		private readonly IUserManager _userManager;
		private readonly IFirebaseUserRepos _authService;

		public AccountController(IUserManager userManager)
		{
			_userManager = userManager;
		}

		// Endpoint for verifying Firebase token from client-side
		[HttpPost("verify-token")]
		public async Task<IActionResult> VerifyToken([FromBody] string idToken)
		{
			try
			{
				// Verify the Firebase ID token
				var verified = await _userManager.VerifyTokenAsync(idToken);

				if (!verified)
				{
					return Unauthorized(new { message = "Invalid or expired token." });
				}

				return Ok(new { message = "Token verified successfully" });
			}
			catch
			{
				return Unauthorized(new { message = "Invalid or expired token." });
			}
		}

		// Sign Up endpoint with registration key validation
		[HttpPost("signup")]
		public async Task<IActionResult> SignUp([FromBody] SignUpRequest request)
		{
			// Validate the registration key
			bool keyValid = await _userManager.UseRegistrationKeyAsync(request.RegistrationKey);
			if (!keyValid)
			{
				return BadRequest(new { message = "Invalid or already used registration key." });
			}

			try
			{
				var user = await FirebaseAuth.DefaultInstance.CreateUserAsync(new UserRecordArgs
				{
					Email = request.Email,
					Password = request.Password,
					DisplayName = request.Name
				});

				// After creating the user, assign role and save to Firestore
				await _authService.AssignRoleAsync(user.Uid, "student");

				return Ok(new { message = "User registered successfully", user });
			}
			catch (FirebaseAuthException ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}

		// Profile endpoint protected by RoleAuth middleware
		[Authorize]
		[HttpGet("profile")]
		public IActionResult Profile()
		{
			var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

			return Ok(new
			{
				Name = User.Identity.Name,
				Email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value,
				Role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value,
				UserId = userId
			});
		}

		// Logout route
		[Authorize]
		[HttpPost("logout")]
		public async Task<IActionResult> Logout()
		{
			await HttpContext.SignOutAsync();
			return Ok(new { message = "Logged out successfully" });
		}
	}

	public class SignUpRequest
	{
		public string Email { get; set; }
		public string Password { get; set; }
		public string Name { get; set; }
		public string RegistrationKey { get; set; }
	}
}
