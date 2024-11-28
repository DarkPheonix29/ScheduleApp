using BLL.Interfaces;
using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.Models;	

namespace BLL.Firebase
{
	public class FirebaseKeyManager : IFirebaseKeyManager
	{
		private readonly FirestoreDb _firestoreDb;

		public FirebaseKeyManager()
		{
			_firestoreDb = FirestoreDb.Create("scheduleapp-819ca"); // Replace with your Firebase project ID
		}

		public async Task<string> GenerateRegistrationKeyAsync()
		{
			var newKey = new
			{
				Key = Guid.NewGuid().ToString(),
				Used = false,
				CreatedAt = Timestamp.FromDateTime(DateTime.UtcNow)
			};

			var docRef = _firestoreDb.Collection("registrationKeys").Document();
			await docRef.SetAsync(newKey);

			return newKey.Key;
		}

		public async Task<bool> ValidateRegistrationKeyAsync(string key)
		{
			var query = _firestoreDb.Collection("registrationKeys").WhereEqualTo("Key", key);
			var snapshot = await query.GetSnapshotAsync();

			if (!snapshot.Documents.Any())
				return false; // Key not found

			var document = snapshot.Documents.First();

			if (document.GetValue<bool>("Used"))
				return false; // Key already used

			// Mark the key as used
			await document.Reference.UpdateAsync("Used", true);

			return true;
		}

		public async Task<List<KeyData>> GetAllKeysAsync()
		{
			var snapshot = await _firestoreDb.Collection("registrationKeys").GetSnapshotAsync();

			return snapshot.Documents
				.Select(doc => doc.ConvertTo<KeyData>())
				.ToList();
		}
	}
}
