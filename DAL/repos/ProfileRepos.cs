using BLL.Interfaces;
using BLL.Models;
using DAL;
using Google.Cloud.Firestore;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DAL.repos
{
	public class ProfileRepos : IProfileRepos
	{
		private readonly ApplicationDbContext _dbContext;
		private readonly FirestoreDb _db;

		public ProfileRepos(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
			_db = FirestoreDb.Create("scheduleapp-819ca");
		}

		public async Task<UserProfile> CreateUserProfileAsync(string email, string displayName, string phoneNumber, string address, string pickupAddress, DateTime dateOfBirth)
		{
			if (string.IsNullOrWhiteSpace(email) ||
				string.IsNullOrWhiteSpace(displayName) ||
				string.IsNullOrWhiteSpace(phoneNumber) ||
				string.IsNullOrWhiteSpace(address) ||
				string.IsNullOrWhiteSpace(pickupAddress) ||
				dateOfBirth == DateTime.MinValue)
			{
				throw new ArgumentException("All fields are required and must not be empty.");
			}

			var userProfile = new UserProfile
			{
				Email = email,
				DisplayName = displayName,
				PhoneNumber = phoneNumber,
				Address = address,
				PickupAddress = pickupAddress,
				DateOfBirth = dateOfBirth
			};

			// Save the user profile to the database
			_dbContext.UserProfiles.Add(userProfile);
			await _dbContext.SaveChangesAsync();

			return userProfile;
		}

		public async Task<List<EmailData>> GetStudentEmailsAsync()
		{
			var emails = new List<EmailData>();

			var snapshot = await _db.Collection("users").GetSnapshotAsync();

			foreach (var document in snapshot.Documents)
			{
				var emailData = document.ConvertTo<EmailData>(); // Convert Firestore document to a C# object
				emailData.Id = document.Id; // Get document ID
				if (emailData.Role == "student")
				{
					emails.Add(emailData);
				}
			}

			return emails;
		}

		public async Task<UserProfile> GetStudentProfileByEmailAsync(string email)
		{
			// Search for the user profile by email in the SQLite database
			var userProfile = await _dbContext.UserProfiles
											   .FirstOrDefaultAsync(up => up.Email == email);

			return userProfile;  // Return the user profile found
		}

	}
}
