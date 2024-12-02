using BLL.Interfaces;
using BLL.Manager;
using BLL.Models;
using FirebaseAdmin.Auth;

public class UserManager : IUserManager
{
	private readonly IFirebaseTokenRepos _tokenService;
	private readonly IFirebaseKeyRepos _keyService;
	private readonly IFirebaseUserRepos _userService;

	public UserManager(IFirebaseTokenRepos tokenService, IFirebaseKeyRepos keyService, IFirebaseUserRepos userService)
	{
		_tokenService = tokenService;
		_keyService = keyService;
		_userService = userService;
	}

	// Verifying the Firebase token
	public async Task<bool> VerifyTokenAsync(string idToken)
	{
		return await _tokenService.VerifyTokenAsync(idToken);
	}

	// Generating a registration key
	public async Task<string> GenerateRegistrationKeyAsync()
	{
		return await _keyService.GenerateRegistrationKeyAsync();
	}

	// Using a registration key and marking it as used
	public async Task<bool> UseRegistrationKeyAsync(string key)
	{
		return await _keyService.UseRegistrationKeyAsync(key);
	}

	// Fetching all the keys
	public async Task<List<KeyData>> GetAllKeysAsync()
	{
		return await _keyService.GetAllKeysAsync();
	}

	// Getting user profile information
	public async Task<UserProfile> GetUserProfileAsync(string uid)
	{
		return await _userService.GetUserProfileAsync(uid);
	}

	// Logging out a user (revoking their refresh token)
	public async Task LogoutUserAsync(string uid)
	{
		await FirebaseAuth.DefaultInstance.RevokeRefreshTokensAsync(uid);
	}

	// Assigning a role to the user
	public async Task AssignRoleAsync(string userId, string role)
	{
		await _userService.AssignRoleAsync(userId, role); 
	}

	// Retrieve a user's role from Firestore
	public async Task<string> GetRoleFromFirestoreAsync(string email)
	{
		return await _userService.GetRoleFromFirestoreAsync(email);
	}
	public async Task<UserRecord> SignUpAsync(string email, string password, string role)
	{
		return await _userService.SignUpAsync(email, password, role);
	}

	public async Task<string> LogInAsync(string email, string password)
	{
		return await _userService.LogInAsync(email, password);
	}
}
