using FirebaseAdmin.Auth;
using System.Threading.Tasks;
using BLL.Interfaces;

namespace BLL.Manager
{
	public class FirebaseTokenManager : IFirebaseTokenManager
	{
		public async Task<bool> VerifyTokenAsync(string idToken)
		{
			try
			{
				// Verify the ID Token using Firebase Admin SDK
				var decodedToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(idToken);
				return decodedToken is not null;  // Return true if the token is valid
			}
			catch (FirebaseAuthException ex)
			{
				// Log the error if token verification fails
				Console.WriteLine($"Token verification failed: {ex.Message}");
				return false;  // Return false if the token is invalid
			}
		}

		public async Task RevokeTokensAsync(string uid)
		{
			try
			{
				// Revoke the user's refresh tokens if needed
				await FirebaseAuth.DefaultInstance.RevokeRefreshTokensAsync(uid);
			}
			catch (FirebaseAuthException ex)
			{
				Console.WriteLine($"Failed to revoke tokens: {ex.Message}");
			}
		}
	}
}
