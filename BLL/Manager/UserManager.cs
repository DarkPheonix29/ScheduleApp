using BLL.Interfaces;
using BLL.Manager;
using BLL.Models;
using FirebaseAdmin.Auth;
using Grpc.Core;

public class UserManager : IUserManager
{
	private readonly IFirebaseTokenManager _tokenService;
	private readonly IFirebaseKeyManager _keyService;
	private readonly IFirebaseUserRepos _userService;

	public static event EventHandler<AdminMessageEventArgs>? OnAdminMessage;

	public class AdminMessageEventArgs(string message) : EventArgs
	{
		public string Message { get; set; } = message;
	}

	public UserManager(IFirebaseTokenManager tokenService, IFirebaseKeyManager keyService, IFirebaseUserRepos userService)
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

	public Task SendAdminMessage(string message)
	{
		Console.WriteLine($"Sending message: {message}");
		if (OnAdminMessage != null)
		{
			Console.WriteLine($"Number of subscribers: {OnAdminMessage.GetInvocationList().Length}");
			OnAdminMessage.Invoke(this, new AdminMessageEventArgs(message));
		}
		else
		{
			Console.WriteLine("No subscribers for OnAdminMessage.");
		}
		return Task.CompletedTask;
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
