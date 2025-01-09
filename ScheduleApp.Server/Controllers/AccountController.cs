using BLL.Interfaces;
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
using Google.Cloud.Firestore;
using BLL.Firebase;

namespace ScheduleApp.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AccountController : ControllerBase
	{
		private readonly IUserManager _userManager;
		private readonly IProfileRepos _profileRepos;
		private readonly IConfiguration _configuration;
		private readonly IExcelRepos _excelRepos;

		public AccountController(IUserManager userManager, IProfileRepos profileRepos, IConfiguration configuration, IExcelRepos excelRepos)
		{
			_userManager = userManager;
			_profileRepos = profileRepos;
			_configuration = configuration;
			_excelRepos = excelRepos;
		}

		[HttpPost("verify")]
		public async Task<IActionResult> VerifyToken(string idToken)
		{
			try
			{
				var verified = await _userManager.VerifyTokenAsync(idToken);

				if (!verified)
				{
					return Unauthorized(new { message = "Invalid or expired token." });
				}

				var decodedToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(idToken);
				var userId = decodedToken.Uid;
				var email = decodedToken.Claims["email"].ToString();

				var claims = new List<Claim>
				{
					new Claim(ClaimTypes.Name, email),
					new Claim(ClaimTypes.NameIdentifier, userId),
					new Claim(ClaimTypes.Email, email),
					new Claim(ClaimTypes.Role, "student"),
					new Claim(ClaimTypes.Role, "instructor")
				};

				var claimsIdentity = new ClaimsIdentity(claims, "Firebase");

				var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

				await HttpContext.SignInAsync("Firebase", claimsPrincipal, new AuthenticationProperties
				{
					IsPersistent = true,
					ExpiresUtc = DateTime.UtcNow.AddDays(30)
				});

				return Ok(new { message = "Token verified successfully" });
			}
			catch (Exception ex)
			{
				return Unauthorized(new { message = $"Error: {ex.Message}" });
			}
		}

		[HttpPost("signup")]
		public async Task<IActionResult> SignUp([FromBody] SignUpRequest request)
		{
			try
			{
				if (!await _userManager.UseRegistrationKeyAsync(request.RegistrationKey))
				{
					return BadRequest(new { message = "Invalid or already used registration key." });
				}

				var user = await _userManager.SignUpAsync(request.Email, request.Password, "student");

				var userProfile = await _profileRepos.CreateUserProfileAsync(
					request.Email, request.Name, request.PhoneNumber, request.Address, request.PickupAddress, request.DateOfBirth
				);

				var templatePath = _configuration["ExcelTemplate:ExcelTemplatePath"];
				if (!System.IO.File.Exists(templatePath))
				{
					throw new FileNotFoundException($"Template file not found at: {templatePath}");
				}

				var fileBytes = System.IO.File.ReadAllBytes(templatePath);

				Console.WriteLine($"Read {fileBytes.Length} bytes from template file.");

				// Use the new method for saving the instructor card during signup
				await _excelRepos.SaveInstructorCardDuringSignupAsync(request.Email);

				// Verify that the file was saved correctly
				var savedFileBytes = await _excelRepos.GetInstructorCardAsync(request.Email);
				if (savedFileBytes != null && savedFileBytes.Length > 0)
				{
					// Save the instructor card to a file to view it
					System.IO.File.WriteAllBytes("InstructorCard_" + request.Email + ".pdf", savedFileBytes);
					Console.WriteLine($"Instructor card saved successfully to a file for user: {request.Email}");
				}
				else
				{
					throw new Exception("Failed to save the instructor card to the database.");
				}

				return Ok(new { message = "User registered successfully.", user });
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error during signup: {ex.Message}");
				return BadRequest(new { message = ex.Message });
			}
		}



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

		[Authorize]
		[HttpPost("logout")]
		public async Task<IActionResult> Logout()
		{
			await HttpContext.SignOutAsync();
			return Ok(new { message = "Logged out successfully" });
		}

		[HttpGet("students")]
		public async Task<IActionResult> GetStudents()
		{
			try
			{
				var studentEmails = await _profileRepos.GetStudentEmailsAsync();

				if (studentEmails == null || !studentEmails.Any())
				{
					return NotFound(new { message = "No students found." });
				}

				return Ok(studentEmails);
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}

		[HttpGet("studentprofile/{email}")]
		public async Task<IActionResult> GetStudentProfile(string email)
		{
			try
			{
				var studentProfile = await _profileRepos.GetStudentProfileByEmailAsync(email);

				if (studentProfile == null)
				{
					return NotFound(new { message = "Student profile not found." });
				}

				return Ok(studentProfile);
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}
	}

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
