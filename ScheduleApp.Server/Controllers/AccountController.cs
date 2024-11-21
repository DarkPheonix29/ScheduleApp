using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;
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
		private readonly FirebaseAuth _firebaseAuth;
		private readonly FirebaseKey _firebaseKey;

		public AccountController()
		{
			// Initialize Firebase Admin SDK only once
			if (FirebaseApp.DefaultInstance == null)
			{
				FirebaseApp.Create(new AppOptions
				{
					Credential = GoogleCredential.FromFile("C:\\path_to_your_service_account_key.json")
				});
			}
			_firebaseAuth = FirebaseAuth.DefaultInstance;
			_firebaseKey = new FirebaseKey();
		}

		// Endpoint for verifying Firebase token from client-side
		[HttpPost("verify-token")]
		public async Task<IActionResult> VerifyToken([FromBody] string idToken)
		{
			try
			{
				// Verify the Firebase ID token
				var decodedToken = await _firebaseAuth.VerifyIdTokenAsync(idToken);

				// Retrieve the user's email from the decoded token
				var email = decodedToken.Claims.ContainsKey("email") ? decodedToken.Claims["email"].ToString() : string.Empty;

				if (string.IsNullOrEmpty(email))
				{
					return Unauthorized(new { message = "Email is missing from the token." });
				}

				// Use FirebaseRoles to get the role from Firestore
				var firebaseRoles = new FirebaseRoles();
				var role = await firebaseRoles.GetRoleFromFirestoreAsync(email);

				// Create claims identity
				var claims = new ClaimsIdentity();
				claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, decodedToken.Uid));
				claims.AddClaim(new Claim(ClaimTypes.Email, email));
				claims.AddClaim(new Claim(ClaimTypes.Name, decodedToken.Claims.ContainsKey("name") ? decodedToken.Claims["name"].ToString() : string.Empty));

				// Add the role claim
				claims.AddClaim(new Claim(ClaimTypes.Role, role));

				// Create ClaimsPrincipal and sign in the user
				var userPrincipal = new ClaimsPrincipal(claims);
				await HttpContext.SignInAsync(userPrincipal);

				return Ok(new { message = "Token verified successfully", role });
			}
			catch
			{
				return Unauthorized(new { message = "Invalid or expired token." });
			}
		}




		// Sign Up endpoint with registration key validation
		// Sign Up endpoint with registration key validation
		[HttpPost("signup")]
		public async Task<IActionResult> SignUp([FromBody] SignUpRequest request)
		{
			// Validate the registration key
			bool keyValid = await _firebaseKey.UseRegistrationKeyAsync(request.RegistrationKey);
			if (!keyValid)
			{
				return BadRequest(new { message = "Invalid or already used registration key." });
			}

			try
			{
				// Firebase signup process
				var user = await _firebaseAuth.CreateUserAsync(new UserRecordArgs
				{
					Email = request.Email,
					Password = request.Password,
					DisplayName = request.Name
				});

				// Assign the 'student' role after creating the user
				var firebaseRoles = new FirebaseRoles();
				await firebaseRoles.AssignRoleAsync(user.Uid, "student");

				// Create Firestore document for the user
				var firestoreDb = FirestoreDb.Create("scheduleapp-819ca"); // Set your Firestore project ID
				var userRef = firestoreDb.Collection("users").Document(user.Uid);

				// Set the user data (role and email)
				await userRef.SetAsync(new
				{
					role = "student",  // Set role to 'student'
					email = request.Email
				});

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
