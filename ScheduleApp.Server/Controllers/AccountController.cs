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
				// Validate and use the registration key
				if (!await _userManager.UseRegistrationKeyAsync(request.RegistrationKey))
				{
					return BadRequest(new { message = "Invalid or already used registration key." });
				}

				// Create the user account
				var user = await _userManager.SignUpAsync(request.Email, request.Password, "student");

				// Set up the user profile
				var userProfile = await _profileRepos.CreateUserProfileAsync(
					request.Email,
					request.Name,
					request.PhoneNumber,
					request.Address,
					request.PickupAddress,
					request.DateOfBirth
				);

				// Predefined data for Excel initialization
				var categories = new List<string>
		{
			"", "Toets verantwoorde verkeerdeelname", "38", "37", "36", "35", "34", "33", "32", "31",
			"Toets Complexe Verkeerssituaties", "30", "29", "28", "27", "26", "25", "24",
			"Toets Eenvoudige Verkeerssituatie / bijzonder verrichtingen", "23", "22", "21", "20", "19", "18", "17", "16", "15", "14", "13", "12", "11", "10", "9",
			"Toets Voertuigbediening en -Beheersing", "8", "7", "6", "5", "4", "3", "2", "1"
		};

				var topics = new List<string>
		{
			"", "", "Zelfstandig", "Milieu", "Besluitvaardig", "Sociaal", "Noodstop", "Weer", "Nacht", "Speciaal", "",
			"Kijktechniek", "Weggedeelten", "Erf", "Rotonde", "Autosnelweg", "Inhalen", "Rijstroken", "",
			"BV", "BV", "BV", "BV", "BV", "BV", "BV", "Afslaan", "Kruispunten", "Tegemoetkomen", "Ruimtekussen", "Rijstroken", "Positie", "Wegrijden", "Kijken", "",
			"Remmen", "Schakelen", "Gas", "Sturen", "Kijken", "Starten", "Houding", "Controle"
		};

				var subtopics = new List<string>
		{
			"", "", "rijden met navigatiesysteem - ritvoorbereiding - routeplanning",
			"milieubewust rijgedrag (HNR)", "aangepast en besluitvaardig rijden", "sociaal en defensief rijden",
			"rem/stuurtechnieken - remmen/uitwijken - noodstop - bermrijden",
			"rijden bij zijwind - mist - regen - sneeuw - ijzel", "rijden bij nacht - schemer",
			"rijden bij speciale situaties (brug - viaduct - tunnel)", "",
			"herhaling kijktechniek", "vop's - overwegen - tram/bushalten", "erf en 30 km-zones",
			"naderen, oprijden en verlaten (1/4 - 1/2 - 3/4 - 4/4)", "invoegen/uitvoegen - rijden op auto(snel)wegen",
			"inhalen - voorbijgaan", "rijstrook wisselen - zijdelingse verplaatsing", "",
			"hellingproef", "vakparkeren (voorwaarts/achterwaarts - haaks/shuin)", "fileparkeren (voorwaarts/achterwaarts)",
			"omkeren door te steken", "halve draai", "bocht achteruitrijden", "recht achteruitrijden",
			"afslaan naar links en rechts", "naderen en oversteken van diverse kruispunten",
			"tegemoetkomen - ingehaald worden", "volgafstand en zijdelingse afstand houden",
			"gebruik van rijstroken - voorsorteerstroken", "positie - volgen van bochten (links en rechts)",
			"wegrijden - verlaten van uitrit - inrijden van uitrit", "kijktechniek - gebruik van spiegels", "",
			"vertragen - remmen - stoppen", "ontkoppelen - schakelen - koppelen", "doseren gaspedaal - gasgeven",
			"stuurbehandeling - stuurtechnieken", "kijktechniek - scan - gebruik spiegels",
			"in/uitstappen - motor starten/afzetten", "zit/stuurhouding - autogordel - hoofdsteun - afstellen spiegels",
			"voorbereidings/controlehandelingen (buiten en binnen de auto)"
		};

				// Prepare the list of data tuples
				var data = new List<(string Category, string Topic, string Subtopic)>();

				for (int i = 0; i < categories.Count; i++)
				{
					var category = categories.ElementAtOrDefault(i) ?? "";
					var topic = topics.ElementAtOrDefault(i) ?? "";
					var subtopic = subtopics.ElementAtOrDefault(i) ?? "";

					// Add the tuple to the list without calling InitializeExcelDataForProfileAsync here
					data.Add((category, topic, subtopic));
				}

				// Call InitializeExcelDataForProfileAsync once, outside the loop
				await _excelRepos.InitializeExcelDataForProfileAsync(userProfile.ProfileId, data);

				// Return success response
				return Ok(new { message = "User signed up successfully." });
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
