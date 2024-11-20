using Google.Cloud.Firestore;

public class FirebaseKey
{
	private readonly FirestoreDb _db;

	public FirebaseKey()
	{
		_db = FirestoreDb.Create("scheduleapp-819ca");
	}

	// Generate a one-time use registration key
	public async Task<string> GenerateRegistrationKeyAsync()
	{
		var key = Guid.NewGuid().ToString();  // Random unique key
		var keyRef = _db.Collection("registrationKeys").Document(key);

		var keyDoc = new Dictionary<string, object>
		{
			{ "key", key },
			{ "used", false },
			{ "createdAt", Timestamp.GetCurrentTimestamp() }
		};

		await keyRef.SetAsync(keyDoc);
		return key;
	}

	// Verify the key and mark it as used
	public async Task<bool> UseRegistrationKeyAsync(string key)
	{
		var keyRef = _db.Collection("registrationKeys").Document(key);
		var keyDoc = await keyRef.GetSnapshotAsync();

		if (keyDoc.Exists && !keyDoc.GetValue<bool>("used"))
		{
			// Mark as used
			await keyRef.UpdateAsync(new Dictionary<string, object> { { "used", true } });
			return true;
		}

		return false; // Invalid or already used key
	}
}