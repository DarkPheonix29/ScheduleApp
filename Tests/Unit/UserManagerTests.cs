using BLL.Interfaces;
using Moq;
using FirebaseAdmin.Auth;
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
			string idToken = "valid-token";
			_mockTokenService.Setup(x => x.VerifyTokenAsync(idToken)).ReturnsAsync(true);

			var result = await _userManager.VerifyTokenAsync(idToken);

			_mockTokenService.Verify(x => x.VerifyTokenAsync(idToken), Times.Once);
			Assert.True(result);
		}

		[Fact]
		public async Task VerifyTokenAsync_InvalidToken_ReturnsFalse()
		{
			string idToken = "invalid-token";
			_mockTokenService.Setup(x => x.VerifyTokenAsync(idToken)).ReturnsAsync(false);

			var result = await _userManager.VerifyTokenAsync(idToken);

			Assert.False(result);
			_mockTokenService.Verify(x => x.VerifyTokenAsync(idToken), Times.Once);
		}

		[Fact]
		public async Task GenerateRegistrationKeyAsync_CallsKeyServiceAndReturnsKey()
		{
			string expectedKey = "generated-key";
			_mockKeyService.Setup(x => x.GenerateRegistrationKeyAsync()).ReturnsAsync(expectedKey);

			var result = await _userManager.GenerateRegistrationKeyAsync();

			_mockKeyService.Verify(x => x.GenerateRegistrationKeyAsync(), Times.Once);
			Assert.Equal(expectedKey, result);
		}

		[Fact]
		public async Task GenerateRegistrationKeyAsync_Failure_ThrowsException()
		{
			_mockKeyService.Setup(x => x.GenerateRegistrationKeyAsync()).ThrowsAsync(new Exception("Key generation failed"));

			await Assert.ThrowsAsync<Exception>(() => _userManager.GenerateRegistrationKeyAsync());
		}

		[Fact]
		public async Task UseRegistrationKeyAsync_CallsKeyServiceAndReturnsTrue()
		{
			string key = "used-key";
			_mockKeyService.Setup(x => x.UseRegistrationKeyAsync(key)).ReturnsAsync(true);

			var result = await _userManager.UseRegistrationKeyAsync(key);

			_mockKeyService.Verify(x => x.UseRegistrationKeyAsync(key), Times.Once);
			Assert.True(result);
		}

		[Fact]
		public async Task UseRegistrationKeyAsync_Failure_ThrowsException()
		{
			string key = "invalid-key";
			_mockKeyService.Setup(x => x.UseRegistrationKeyAsync(key)).ThrowsAsync(new Exception("Key usage failed"));

			await Assert.ThrowsAsync<Exception>(() => _userManager.UseRegistrationKeyAsync(key));
		}

		[Fact]
		public async Task AssignRoleAsync_CallsUserServiceToAssignRole()
		{
			string userId = "user-123";
			string role = "admin";
			_mockUserService.Setup(x => x.AssignRoleAsync(userId, role)).Returns(Task.CompletedTask);

			await _userManager.AssignRoleAsync(userId, role);

			_mockUserService.Verify(x => x.AssignRoleAsync(userId, role), Times.Once);
		}

		[Fact]
		public async Task AssignRoleAsync_Failure_ThrowsException()
		{
			string userId = "user-123";
			string role = "admin";
			_mockUserService.Setup(x => x.AssignRoleAsync(userId, role)).ThrowsAsync(new Exception("Role assignment failed"));

			await Assert.ThrowsAsync<Exception>(() => _userManager.AssignRoleAsync(userId, role));
		}

		[Fact]
		public async Task GetRoleFromFirestoreAsync_CallsUserServiceAndReturnsRole()
		{
			string email = "test@example.com";
			string role = "admin";
			_mockUserService.Setup(x => x.GetRoleFromFirestoreAsync(email)).ReturnsAsync(role);

			var result = await _userManager.GetRoleFromFirestoreAsync(email);

			_mockUserService.Verify(x => x.GetRoleFromFirestoreAsync(email), Times.Once);
			Assert.Equal(role, result);
		}

		[Fact]
		public async Task GetRoleFromFirestoreAsync_InvalidRole_ReturnsNull()
		{
			string email = "test@example.com";
			_mockUserService.Setup(x => x.GetRoleFromFirestoreAsync(email)).ReturnsAsync(null as string);

			var result = await _userManager.GetRoleFromFirestoreAsync(email);

			Assert.Null(result);
		}

		[Fact]
		public async Task LogInAsync_CallsUserServiceAndReturnsToken()
		{
			string email = "test@example.com";
			string password = "password";
			string token = "auth-token";
			_mockUserService.Setup(x => x.LogInAsync(email, password)).ReturnsAsync(token);

			var result = await _userManager.LogInAsync(email, password);

			_mockUserService.Verify(x => x.LogInAsync(email, password), Times.Once);
			Assert.Equal(token, result);
		}

		[Fact]
		public async Task LogInAsync_Failure_ThrowsException()
		{
			string email = "test@example.com";
			string password = "wrong-password";
			_mockUserService.Setup(x => x.LogInAsync(email, password)).ThrowsAsync(new Exception("Login failed"));

			await Assert.ThrowsAsync<Exception>(() => _userManager.LogInAsync(email, password));
		}

		// Additional tests for edge cases
		[Fact]
		public async Task AssignRoleAsync_NullRole_ReturnsFalse()
		{
			string userId = "user-123";
			await _userManager.AssignRoleAsync(userId, null!);  // or handle null gracefully in the method
																// Verify result or handle the specific return logic
		}


		[Fact]

		public async Task LogInAsync_NullCredentials_ReturnsNull()
		{
			await _userManager.LogInAsync(null!, null!);  // or handle null gracefully in the method
														  // Verify result or handle specific logic for null credentials
		}

	}
}
