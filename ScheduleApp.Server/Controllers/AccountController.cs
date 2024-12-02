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

		// Inject IFirebaseUserRepos into the constructor
		public AccountController(IUserManager userManager, IFirebaseUserRepos authService)
		{
			_userManager = userManager;
			_authService = authService;
		}

		// Endpoint for verifying Firebase token from client-side
		[HttpPost("verify")]
		public async Task<IActionResult> VerifyToken(string idToken)
		{
			try
			{
				// Verify the Firebase ID token
				var verified = await _userManager.VerifyTokenAsync(idToken);

				if (!verified)
				{
					return Unauthorized(new { message = "Invalid or expired token." });
				}

				// Retrieve user info from Firebase (e.g., email, UID)
				var decodedToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(idToken);
				var userId = decodedToken.Uid;
				var email = decodedToken.Claims["email"].ToString(); // Correct way to access email from claims

				// Create claims identity for ASP.NET Core authentication
				var claims = new List<Claim>
		{
			new Claim(ClaimTypes.Name, email),
			new Claim(ClaimTypes.NameIdentifier, userId),
			new Claim(ClaimTypes.Email, email),
			new Claim(ClaimTypes.Role, "student")  // Ensure role is assigned
        };

				var claimsIdentity = new ClaimsIdentity(claims, "Firebase");

				// Create the authentication ticket
				var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

				// Sign in the user and set the authentication cookie
				await HttpContext.SignInAsync("Firebase", claimsPrincipal, new AuthenticationProperties
				{
					IsPersistent = true, // Keep the user logged in even after closing the browser
					ExpiresUtc = DateTime.UtcNow.AddDays(30) // Set expiration time for the cookie
				});

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
				// Create the user in Firebase
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
				Name = User.Identity?.Name,
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
