using BLL.Models;


namespace BLL.Interfaces
{
	public interface IFirebaseUserRepos
	{
		Task<UserProfile> GetUserProfileAsync(string uid);
		Task LogoutUserAsync(string uid);
		Task AssignRoleAsync(string userId, string role);
		Task<string> GetRoleFromFirestoreAsync(string email);
	}
}
