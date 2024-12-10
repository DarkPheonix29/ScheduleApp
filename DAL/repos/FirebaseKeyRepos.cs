using BLL.Interfaces;
using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.Models;	

namespace BLL.Firebase
{
	public class FirebaseKeyRepos : IFirebaseKeyManager
	{
		private readonly FirestoreDb _db;

		public FirebaseKeyRepos()
		{
			_db = FirestoreDb.Create("scheduleapp-819ca"); // Firebase project ID
		}

		// Generate a one-time use registration key
		public async Task<string> GenerateRegistrationKeyAsync()
		{
			var key = Guid.NewGuid().ToString(); // Generate a unique GUID

			// Use the key as the document ID in Firestore
			var keyRef = _db.Collection("registrationKeys").Document(key);

			// Set the initial key data
			var keyDoc = new Dictionary<string, object>
		{
			{ "key", key },
			{ "used", false },
			{ "createdAt", Timestamp.GetCurrentTimestamp() }
		};

			await keyRef.SetAsync(keyDoc);
			return key; // Return the generated key
		}

		// Verify the key and mark it as used
		public async Task<bool> UseRegistrationKeyAsync(string key)
		{
			var keyRef = _db.Collection("registrationKeys").Document(key); // Use key as document ID
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
				var keyData = document.ConvertTo<KeyData>(); // Automatically map fields to KeyData
				keyData.Id = document.Id; // Set the document ID as 'Id' (which is the same as 'key')
				keys.Add(keyData);
			}

			return keys;
		}

		private readonly FirestoreDb _firestoreDb;

		// Constructor initializes Firestore database connection
		// Method to generate a new registration key
		public async Task<string> GenerateKeyAsync()
		{
			// Create a new key
			var newKey = new
			{
				Key = Guid.NewGuid().ToString(), // Generate a unique key using GUID
				Used = false,
				CreatedAt = Timestamp.FromDateTime(DateTime.UtcNow)
			};

			// Add the key to the Firestore collection
			var docRef = _firestoreDb.Collection("registrationKeys").Document();
			await docRef.SetAsync(newKey);

			return newKey.Key;
		}

		// Method to fetch all keys from Firestore
		public async Task<List<KeyData>> FetchKeysAsync()
		{
			var keys = new List<KeyData>();

			// Retrieve all documents from the "registrationKeys" collection
			var snapshot = await _firestoreDb.Collection("registrationKeys").GetSnapshotAsync();

			foreach (var document in snapshot.Documents)
			{
				var keyData = document.ConvertTo<KeyData>(); // Convert Firestore document to a C# object
				keyData.Id = document.Id; // Get document ID
				keys.Add(keyData);
			}

			return keys;
		}

		// Method to mark a key as used
		public async Task<bool> MarkKeyAsUsedAsync(string key)
		{
			var query = _firestoreDb.Collection("registrationKeys").WhereEqualTo("Key", key);
			var snapshot = await query.GetSnapshotAsync();

			if (snapshot.Documents.Count == 0)
			{
				return false; // Key not found
			}

			var document = snapshot.Documents.First();
			var docRef = document.Reference;

			// Update the 'Used' field to true
			await docRef.UpdateAsync("Used", true);

			return true;
		}
	}
}
