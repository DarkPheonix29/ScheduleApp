using FirebaseAdmin.Auth;
using Google.Cloud.Firestore;
using System.Collections.Generic;
using System.Threading.Tasks;

public class FirebaseRoles
{
	private readonly FirebaseAuth _auth;
	private readonly FirestoreDb _firestoreDb;

	public FirebaseRoles()
	{
		_auth = FirebaseAuth.DefaultInstance; // Use the default instance of FirebaseAuth
		_firestoreDb = FirestoreDb.Create("scheduleapp-819ca"); // Set your Firestore project ID
	}

	// Assign a role to a user
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