﻿using BLL.Interfaces;
using BLL.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FirebaseAdmin.Auth;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System;
using BLL.Manager;

namespace ScheduleApp.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AccountController : ControllerBase
	{
		private readonly IUserManager _userManager;
		private readonly IProfileRepos _profileRepos;  // Inject IProfileRepos

		// Inject IUserManager and IProfileRepos into the constructor
		public AccountController(IUserManager userManager, IProfileRepos profileRepos)
		{
			_userManager = userManager;
			_profileRepos = profileRepos;
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
			try
			{
				// Validate registration key
				bool keyValid = await _userManager.UseRegistrationKeyAsync(request.RegistrationKey);
				if (!keyValid)
				{
					return BadRequest(new { message = "Invalid or already used registration key." });
				}

				// Create user in Firebase
				var user = await _userManager.SignUpAsync(request.Email, request.Password, "student");

				// Save user profile data in SQLite (excluding password)
				UserProfile userProfile = await _profileRepos.CreateUserProfileAsync(request.Email, request.Name, request.PhoneNumber, request.Address, request.PickupAddress, request.DateOfBirth);

				return Ok(new { message = "User registered successfully", user });
			}
			catch (Exception ex)
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

	// Sign-up request model to hold user data from frontend
	public class SignUpRequest
	{
		public string Email { get; set; }
		public string Password { get; set; }
		public string Name { get; set; }
		public string RegistrationKey { get; set; }
		public string PhoneNumber { get; set; }
		public string Address { get; set; }
		public string PickupAddress { get; set; }
		public DateTime DateOfBirth { get; set; }
	}
}
