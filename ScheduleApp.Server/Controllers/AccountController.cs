using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ScheduleApp.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AccountController : ControllerBase
	{
		private readonly FirebaseAuth _firebaseAuth;

		public AccountController()
		{
			// Initialize Firebase Admin SDK only once (Ensure this is done before any Firebase operations)
			if (FirebaseApp.DefaultInstance == null)
			{
				FirebaseApp.Create(new AppOptions()
				{
					Credential = GoogleCredential.FromFile("C:\\Users\\keala\\source\\repos\\ScheduleApp\\scheduleapp-819ca-firebase-adminsdk-hj5ct-ed6b7912e0.json")
				});
			}
			_firebaseAuth = FirebaseAuth.DefaultInstance;
		}

		// This will be handled client-side using Firebase SDK
		[HttpGet("login")]
		public IActionResult Login(string returnUrl = "/")
		{
			// Normally, login is done on the front end via Firebase SDK (no need for backend login route)
			// Redirecting to the frontend for Firebase login (this would be handled by Firebase JS SDK on the client)
			return Redirect(returnUrl);
		}

		// Endpoint for verifying Firebase token from client-side
		[HttpPost("verify-token")]
		public async Task<IActionResult> VerifyToken([FromBody] string idToken)
		{
			try
			{
				// Verify the Firebase ID token
				var decodedToken = await _firebaseAuth.VerifyIdTokenAsync(idToken);

				// Create a ClaimsIdentity and add claims from Firebase token
				var userClaims = new ClaimsIdentity();
				userClaims.AddClaim(new Claim(ClaimTypes.NameIdentifier, decodedToken.Uid));

				// Add other claims from Firebase token
				if (decodedToken.Claims.ContainsKey("name"))
					userClaims.AddClaim(new Claim(ClaimTypes.Name, decodedToken.Claims["name"].ToString()));

				if (decodedToken.Claims.ContainsKey("email"))
					userClaims.AddClaim(new Claim(ClaimTypes.Email, decodedToken.Claims["email"].ToString()));

				if (decodedToken.Claims.ContainsKey("picture"))
					userClaims.AddClaim(new Claim("picture", decodedToken.Claims["picture"].ToString()));

				// Optionally: Add roles based on Firebase custom claims
				var role = decodedToken.Claims.ContainsKey("role") ? decodedToken.Claims["role"].ToString() : "Guest";
				userClaims.AddClaim(new Claim("role", role));

				var userPrincipal = new ClaimsPrincipal(userClaims);
				await HttpContext.SignInAsync(userPrincipal);

				return Ok(new { message = "Token verified successfully" });
			}
			catch (Exception ex)
			{
				return Unauthorized(new { message = "Invalid token", error = ex.Message });
			}
		}

		// Profile route, protected by authorization (requires the user to be authenticated)
		[Authorize]
		[HttpGet("profile")]
		public IActionResult Profile()
		{
			// Firebase UID will be available after token verification in middleware
			var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

			return new JsonResult(new
			{
				Name = User.Identity.Name, // This will be populated if you set it in your Firebase token
				EmailAddress = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value,
				ProfileImage = User.Claims.FirstOrDefault(c => c.Type == "picture")?.Value,
				UserId = userId
			});
		}

		// Logout - handled by Firebase client SDK, but you can clear session here if needed
		[Authorize]
		[HttpPost("logout")]
		public async Task Logout()
		{
			// Firebase logout is handled client-side by Firebase SDK
			// Optionally: Clear session or authentication token from server
			await HttpContext.SignOutAsync();
		}
	}
}
