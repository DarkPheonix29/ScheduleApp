using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class FirebaseKey
{
	private readonly FirestoreDb _db;

	public FirebaseKey()
	{
		_db = FirestoreDb.Create("scheduleapp-819ca"); // Firebase project ID
	}

	// Generate a one-time use registration key
	public async Task<string> GenerateRegistrationKeyAsync()
	{
		var key = Guid.NewGuid().ToString(); // Generate a unique GUID
		var keyRef = _db.Collection("registrationKeys").Document(key);

		// Set the initial key data
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
			// Mark the key as used
			await keyRef.UpdateAsync(new Dictionary<string, object> { { "used", true } });
			return true;
		}

		return false; // Key not found or already used
	}

	// Fetch all keys (optional for administration purposes)
	public async Task<List<KeyData>> GetAllKeysAsync()
	{
		var keys = new List<KeyData>();
		var snapshot = await _db.Collection("registrationKeys").GetSnapshotAsync();

		foreach (var document in snapshot.Documents)
		{
			var keyData = document.ConvertTo<KeyData>(); // Map to KeyData
			keyData.Id = document.Id; // Document ID for reference
			keys.Add(keyData);
		}

		return keys;
	}
}

public class KeyData
{
	public string Key { get; set; }
	public bool Used { get; set; }
	public Timestamp CreatedAt { get; set; }
	public string Id { get; set; } // Document ID for future reference
}