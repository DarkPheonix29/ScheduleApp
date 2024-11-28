using BLL.Interfaces;
using BLL.Models;
using FirebaseAdmin.Auth;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.Manager
{
	public class UserManager : IUserManager
	{
		private readonly IFirebaseTokenManager _tokenService;
		private readonly IFirebaseKeyManager _keyService;
		private readonly IFirebaseUserRepos _userService;

		public UserManager(IFirebaseTokenManager tokenService, IFirebaseKeyManager keyService, IFirebaseUserRepos userService)
		{
			_tokenService = tokenService;
			_keyService = keyService;
			_userService = userService;
		}

		public async Task<bool> VerifyTokenAsync(string idToken)
		{
			return await _tokenService.VerifyTokenAsync(idToken);
		}

		public async Task<string> GenerateRegistrationKeyAsync()
		{
			return await _keyService.GenerateRegistrationKeyAsync();
		}

		public async Task<bool> UseRegistrationKeyAsync(string key)
		{
			return await _keyService.ValidateRegistrationKeyAsync(key);
		}

		public async Task<List<KeyData>> GetAllKeysAsync()
		{
			return await _keyService.GetAllKeysAsync();
		}

		public async Task<UserProfile> GetUserProfileAsync(string uid)
		{
			return await _userService.GetUserProfileAsync(uid);
		}

		public async Task LogoutUserAsync(string uid)
		{
			await FirebaseAuth.DefaultInstance.RevokeRefreshTokensAsync(uid);
		}
	}
}
