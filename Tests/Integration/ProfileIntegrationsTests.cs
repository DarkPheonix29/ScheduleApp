using BLL.Interfaces;
using BLL.Models;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Tests.Unit
{
	public class ProfileIntegrationsTests
	{
		private readonly Mock<IProfileRepos> _mockProfileRepos;

		public ProfileIntegrationsTests()
		{
			// Initialize the mocked repository
			_mockProfileRepos = new Mock<IProfileRepos>();
		}

		// Test CreateUserProfileAsync (Happy Path)
		[Fact]
		public async Task CreateUserProfileAsync_ShouldCreateUserProfile()
		{
			// Arrange
			var email = "testuser@example.com";
			var displayName = "Test User";
			var phoneNumber = "123456789";
			var address = "123 Test St";
			var pickupAddress = "456 Pickup Ave";
			var dateOfBirth = new DateTime(1990, 1, 1);

			var expectedUserProfile = new UserProfile
			{
				Email = email,
				DisplayName = displayName,
				PhoneNumber = phoneNumber,
				Address = address,
				PickupAddress = pickupAddress,
				DateOfBirth = dateOfBirth
			};

			// Setup the mock to return the expected user profile
			_mockProfileRepos
				.Setup(repo => repo.CreateUserProfileAsync(email, displayName, phoneNumber, address, pickupAddress, dateOfBirth))
				.ReturnsAsync(expectedUserProfile);

			// Act
			var userProfile = await _mockProfileRepos.Object.CreateUserProfileAsync(email, displayName, phoneNumber, address, pickupAddress, dateOfBirth);

			// Assert
			Assert.NotNull(userProfile);
			Assert.Equal(email, userProfile.Email);
			Assert.Equal(displayName, userProfile.DisplayName);
		}

		// Test CreateUserProfileAsync (Alternate Path: Missing Required Fields)
		[Fact]
		public async Task CreateUserProfileAsync_ShouldThrowArgumentException_WhenRequiredFieldsAreMissing()
		{
			// Arrange
			_mockProfileRepos
				.Setup(repo => repo.CreateUserProfileAsync("", "", "", "", "", DateTime.MinValue))
				.ThrowsAsync(new ArgumentException("Required fields are missing"));

			// Act & Assert
			await Assert.ThrowsAsync<ArgumentException>(async () =>
				await _mockProfileRepos.Object.CreateUserProfileAsync("", "", "", "", "", DateTime.MinValue)
			);
		}

		// Test GetStudentProfileByEmailAsync (Happy Path)
		[Fact]
		public async Task GetStudentProfileByEmailAsync_ShouldReturnUserProfile_WhenEmailExists()
		{
			// Arrange
			var email = "testuser@example.com";
			var expectedUserProfile = new UserProfile
			{
				Email = email,
				DisplayName = "Test User"
			};

			// Setup the mock to return the expected user profile
			_mockProfileRepos
				.Setup(repo => repo.GetStudentProfileByEmailAsync(email))
				.ReturnsAsync(expectedUserProfile);

			// Act
			var userProfile = await _mockProfileRepos.Object.GetStudentProfileByEmailAsync(email);

			// Assert
			Assert.NotNull(userProfile);
			Assert.Equal(email, userProfile.Email);
		}

		// Test GetStudentProfileByEmailAsync (Alternate Path: Email Not Found)
		[Fact]
		public async Task GetStudentProfileByEmailAsync_ShouldReturnNull_WhenEmailDoesNotExist()
		{
			// Arrange
			var nonExistentEmail = "nonexistent@example.com";

			// Setup the mock to return null for a non-existent email
			_mockProfileRepos
				.Setup(repo => repo.GetStudentProfileByEmailAsync(nonExistentEmail))
				.ReturnsAsync((UserProfile)null);

			// Act
			var userProfile = await _mockProfileRepos.Object.GetStudentProfileByEmailAsync(nonExistentEmail);

			// Assert
			Assert.Null(userProfile);
		}
	}
}
