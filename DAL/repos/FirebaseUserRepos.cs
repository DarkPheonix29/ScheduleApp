using BLL.Interfaces;
using BLL.Models;
using DAL;
using FirebaseAdmin.Auth;
using Google;

using Microsoft.EntityFrameworkCore;

namespace BLL.Firebase
{
	public class FirebaseUserRepos : IFirebaseUserRepos
	{
		private readonly ApplicationDbContext _dbContext;

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
	}
}
