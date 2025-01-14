using BLL.Interfaces;
using BLL.Models;
using DAL.repos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Tests.Unit
{
	public class ProfileIntegrationsTests
	{
		private readonly IProfileRepos _profileRepos;
		private readonly ApplicationDbContext _dbContext;

		public ProfileIntegrationsTests()
		{
			// Use SQLite In-Memory Database for testing
			var options = new DbContextOptionsBuilder<ApplicationDbContext>()
				.UseInMemoryDatabase(databaseName: "TestDatabase")
				.Options;

			_dbContext = new ApplicationDbContext(options);
			_profileRepos = new ProfileRepos(_dbContext); // Directly use ApplicationDbContext for the test
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

			// Act
			var userProfile = await _profileRepos.CreateUserProfileAsync(email, displayName, phoneNumber, address, pickupAddress, dateOfBirth);

			// Assert
			Assert.NotNull(userProfile);
			Assert.Equal(email, userProfile.Email);
			Assert.Equal(displayName, userProfile.DisplayName);
		}

		// Test CreateUserProfileAsync (Alternate Path: Missing Required Fields)
		[Fact]
		public async Task CreateUserProfileAsync_ShouldThrowArgumentException_WhenRequiredFieldsAreMissing()
		{
			// Act & Assert
			await Assert.ThrowsAsync<ArgumentException>(async () =>
				await _profileRepos.CreateUserProfileAsync("", "", "", "", "", DateTime.MinValue)
			);
		}

		// Test GetStudentProfileByEmailAsync (Happy Path)
		[Fact]
		public async Task GetStudentProfileByEmailAsync_ShouldReturnUserProfile_WhenEmailExists()
		{
			// Arrange
			var email = "testuser@example.com";
			var createdProfile = await _profileRepos.CreateUserProfileAsync(email, "Test User", "123456789", "123 St", "456 Pickup Ave", new DateTime(1990, 1, 1));

			// Act
			var userProfile = await _profileRepos.GetStudentProfileByEmailAsync(email);

			// Assert
			Assert.NotNull(userProfile);
			Assert.Equal(email, userProfile.Email);
		}

		// Test GetStudentProfileByEmailAsync (Alternate Path: Email Not Found)
		[Fact]
		public async Task GetStudentProfileByEmailAsync_ShouldReturnNull_WhenEmailDoesNotExist()
		{
			// Act
			var userProfile = await _profileRepos.GetStudentProfileByEmailAsync("nonexistent@example.com");

			// Assert
			Assert.Null(userProfile);
		}
	}
}
