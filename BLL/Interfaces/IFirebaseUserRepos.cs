using BLL.Models;
using FirebaseAdmin.Auth;
using Google.Cloud.Firestore;


namespace BLL.Interfaces
{
	public interface IFirebaseUserRepos
	{
		Task<UserProfile> GetUserProfileAsync(string uid);
		Task LogoutUserAsync(string uid);
		Task AssignRoleAsync(string userId, string role);
		Task<string> GetRoleFromFirestoreAsync(string email);
		Task<UserRecord> SignUpAsync(string email, string password, string role);
		Task<string> LogInAsync(string email, string password);
	}
}
