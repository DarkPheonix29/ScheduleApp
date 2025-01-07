using BLL.Firebase;
using BLL.Models;
using Google.Cloud.Firestore;
using Xunit;
using System.Threading.Tasks;
using System.Collections.Generic;
using FirebaseAdmin.Auth;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;

namespace Tests.Integration
{
	public class FirebaseKeyReposTests
	{
		private readonly FirebaseUserRepos _userrepo;
		private readonly FirebaseKeyRepos _keyrepo;
		private readonly FirestoreDb _db;
		private readonly FirebaseAuth _auth;

		public FirebaseKeyReposTests()
		{
			if (FirebaseApp.DefaultInstance == null)
			{
				FirebaseApp.Create(new AppOptions()
				{
					Credential = GoogleCredential.GetApplicationDefault()
				});
			}

			_db = FirestoreDb.Create("scheduleapp-819ca"); // Firebase project ID
			_keyrepo = new FirebaseKeyRepos();
			_auth = FirebaseAuth.DefaultInstance;
		}

		[Fact]
		public async Task GenerateRegistrationKeyAsync_CreatesKey()
		{
			// Act
			var key = await _keyrepo.GenerateRegistrationKeyAsync();

			// Assert
			Assert.NotNull(key);
			var keyDoc = await _db.Collection("registrationKeys").Document(key).GetSnapshotAsync();
			Assert.True(keyDoc.Exists);
			Assert.Equal(key, keyDoc.GetValue<string>("key"));
			Assert.False(keyDoc.GetValue<bool>("used"));
		}

		[Fact]
		public async Task UseRegistrationKeyAsync_ValidKey_MarksAsUsed()
		{
			// Arrange
			var key = await _keyrepo.GenerateRegistrationKeyAsync();

			// Act
			var result = await _keyrepo.UseRegistrationKeyAsync(key);

			// Assert
			Assert.True(result);
			var keyDoc = await _db.Collection("registrationKeys").Document(key).GetSnapshotAsync();
			Assert.True(keyDoc.GetValue<bool>("used"));
		}

		[Fact]
		public async Task GetAllKeysAsync_ReturnsAllKeys()
		{
			// Act
			var keys = await _keyrepo.GetAllKeysAsync();

			// Assert
			Assert.NotEmpty(keys);
			Assert.True(keys.Count > 0); // Ensure that keys exist in the collection
		}
	}
}
