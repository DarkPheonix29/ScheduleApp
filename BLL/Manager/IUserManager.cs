using BLL.Models;
using FirebaseAdmin.Auth;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.Manager
{
	public interface IUserManager
	{
		Task<bool> VerifyTokenAsync(string idToken);
		Task<string> GenerateRegistrationKeyAsync();
		Task<bool> UseRegistrationKeyAsync(string key);
		Task<List<KeyData>> GetAllKeysAsync();
		Task<UserProfile> GetUserProfileAsync(string uid);
		Task LogoutUserAsync(string uid);
	}
}
