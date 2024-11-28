using BLL.Interfaces;
using BLL.Models;
using DAL;
using FirebaseAdmin.Auth;
using Google;
using Google.Cloud.Firestore;
using Microsoft.EntityFrameworkCore;

namespace BLL.Firebase
{
	public class FirebaseUserRepos : IFirebaseUserRepos
	{
		private readonly ApplicationDbContext _dbContext;
		private readonly FirebaseAuth _auth;
		private readonly FirestoreDb _firestoreDb;

		public async Task<UserProfile> GetUserProfileAsync(string uid)
		{
			var userProfile = await _dbContext.UserProfiles.FirstOrDefaultAsync(u => u.FirebaseUid == uid);

			if (userProfile == null)
			{
				throw new Exception("User profile not found.");
			}

			return userProfile;
		}

		public async Task LogoutUserAsync(string uid)
		{
			// Optional: Revoke tokens using Firebase Admin SDK
			try
			{
				await FirebaseAuth.DefaultInstance.RevokeRefreshTokensAsync(uid);
			}
			catch (Exception ex)
			{
				throw new Exception("Error revoking tokens: " + ex.Message);
			}
		}
		public async Task AssignRoleAsync(string userId, string role)
		{
			var customClaims = new Dictionary<string, object> { { "role", role } };

			// Set custom claims to the user's Firebase authentication record
			await _auth.SetCustomUserClaimsAsync(userId, customClaims);
		}

		// Retrieve a user's role from Firestore
		public async Task<string> GetRoleFromFirestoreAsync(string email)
		{
			// Reference to the Firestore collection
			var collection = _firestoreDb.Collection("users");
			var query = collection.WhereEqualTo("email", email);
			var snapshot = await query.GetSnapshotAsync();

			if (snapshot.Count > 0)
			{
				// Assuming the first match contains the role field
				var document = snapshot.Documents[0];
				var role = document.GetValue<string>("role");
				return role;
			}

			return "Guest"; // Default role if none is found
		}
	}
}
