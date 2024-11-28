using Google.Cloud.Firestore;
using System;
using BLL.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.Firebase
{
	internal class KeyManager
	{
		private readonly FirestoreDb _firestoreDb;

		// Constructor initializes Firestore database connection
		public KeyManager()
		{
			_firestoreDb = FirestoreDb.Create("scheduleapp-819ca"); // Replace with your Firebase project ID
		}

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

	// Helper class to represent the key data
}