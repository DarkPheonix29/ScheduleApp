using BLL.Manager;
using BLL.Interfaces;
using BLL.Models;
using FirebaseAdmin.Auth;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Tests.Unit
{
	public class UserManagerTests
	{
		private readonly Mock<IFirebaseTokenManager> _mockTokenService;
		private readonly Mock<IFirebaseKeyManager> _mockKeyService;
		private readonly Mock<IFirebaseUserRepos> _mockUserService;
		private readonly UserManager _userManager;

		public UserManagerTests()
		{
			_mockTokenService = new Mock<IFirebaseTokenManager>();
			_mockKeyService = new Mock<IFirebaseKeyManager>();
			_mockUserService = new Mock<IFirebaseUserRepos>();
			_userManager = new UserManager(_mockTokenService.Object, _mockKeyService.Object, _mockUserService.Object);
		}

		[Fact]
		public async Task VerifyTokenAsync_CallsTokenServiceAndReturnsExpectedResult()
		{
			// Arrange
			string idToken = "some-token";
			_mockTokenService.Setup(x => x.VerifyTokenAsync(idToken)).ReturnsAsync(true);

			// Act
			var result = await _userManager.VerifyTokenAsync(idToken);

			// Assert
			_mockTokenService.Verify(x => x.VerifyTokenAsync(idToken), Times.Once);
			Assert.True(result);
		}

		[Fact]
		public async Task GenerateRegistrationKeyAsync_CallsKeyServiceAndReturnsKey()
		{
			// Arrange
			string expectedKey = "generated-key";
			_mockKeyService.Setup(x => x.GenerateRegistrationKeyAsync()).ReturnsAsync(expectedKey);

			// Act
			var result = await _userManager.GenerateRegistrationKeyAsync();

			// Assert
			_mockKeyService.Verify(x => x.GenerateRegistrationKeyAsync(), Times.Once);
			Assert.Equal(expectedKey, result);
		}

		[Fact]
		public async Task UseRegistrationKeyAsync_CallsKeyServiceAndReturnsTrue()
		{
			// Arrange
			string key = "used-key";
			_mockKeyService.Setup(x => x.UseRegistrationKeyAsync(key)).ReturnsAsync(true);

			// Act
			var result = await _userManager.UseRegistrationKeyAsync(key);

			// Assert
			_mockKeyService.Verify(x => x.UseRegistrationKeyAsync(key), Times.Once);
			Assert.True(result);
		}

		[Fact]
		public async Task GetAllKeysAsync_CallsKeyServiceAndReturnsListOfKeys()
		{
			// Arrange
			var keys = new List<KeyData> { new KeyData { Key = "key1" }, new KeyData { Key = "key2" } };
			_mockKeyService.Setup(x => x.GetAllKeysAsync()).ReturnsAsync(keys);

			// Act
			var result = await _userManager.GetAllKeysAsync();

			// Assert
			_mockKeyService.Verify(x => x.GetAllKeysAsync(), Times.Once);
			Assert.Equal(keys.Count, result.Count);
		}

		[Fact]
		public async Task GetUserProfileAsync_CallsUserServiceAndReturnsProfile()
		{
			// Arrange
			string uid = "user-123";
			var profile = new UserProfile { FirebaseUid = uid, DisplayName = "John Doe" };
			_mockUserService.Setup(x => x.GetUserProfileAsync(uid)).ReturnsAsync(profile);

			// Act
			var result = await _userManager.GetUserProfileAsync(uid);

			// Assert
			_mockUserService.Verify(x => x.GetUserProfileAsync(uid), Times.Once);
			Assert.Equal(profile.DisplayName, result.DisplayName);
		}

		[Fact]
		public async Task AssignRoleAsync_CallsUserServiceToAssignRole()
		{
			// Arrange
			string userId = "user-123";
			string role = "admin";
			_mockUserService.Setup(x => x.AssignRoleAsync(userId, role)).Returns(Task.CompletedTask);

			// Act
			await _userManager.AssignRoleAsync(userId, role);

			// Assert
			_mockUserService.Verify(x => x.AssignRoleAsync(userId, role), Times.Once);
		}

		[Fact]
		public async Task GetRoleFromFirestoreAsync_CallsUserServiceAndReturnsRole()
		{
			// Arrange
			string email = "test@example.com";
			string role = "admin";
			_mockUserService.Setup(x => x.GetRoleFromFirestoreAsync(email)).ReturnsAsync(role);

			// Act
			var result = await _userManager.GetRoleFromFirestoreAsync(email);

			// Assert
			_mockUserService.Verify(x => x.GetRoleFromFirestoreAsync(email), Times.Once);
			Assert.Equal(role, result);
		}

		[Fact]
		public async Task LogInAsync_CallsUserServiceAndReturnsToken()
		{
			// Arrange
			string email = "test@example.com";
			string password = "password";
			string token = "auth-token";
			_mockUserService.Setup(x => x.LogInAsync(email, password)).ReturnsAsync(token);

			// Act
			var result = await _userManager.LogInAsync(email, password);

			// Assert
			_mockUserService.Verify(x => x.LogInAsync(email, password), Times.Once);
			Assert.Equal(token, result);
		}
	}
}
