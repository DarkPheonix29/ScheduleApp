// Rename FirebaseAuth class to FirebaseRoles
using Google.Cloud.Firestore;

public class FirebaseRoles
{
	private readonly FirebaseAdmin.Auth.FirebaseAuth _auth;

	public FirebaseRoles()
	{
		_auth = FirebaseAdmin.Auth.FirebaseAuth.DefaultInstance;
	}

	// Assign a role to a user
	public async Task AssignRoleAsync(string userId, string role)
	{
		var user = await _auth.GetUserAsync(userId);
		if (user != null)
		{
			var customClaims = new Dictionary<string, object>
			{
				{ "role", role }
			};

			// Set custom claims (role) for the user
			await _auth.SetCustomUserClaimsAsync(userId, customClaims);
		}
	}
}


