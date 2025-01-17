public class UserProfileTests
{
	[Fact]
	public void UserProfile_ShouldHaveCorrectNavigationProperties()
	{
		// Arrange
		var userProfile = new UserProfile
		{
			ProfileId = 1,
			Email = "test@example.com",
			DisplayName = "Test User",
			PhoneNumber = "1234567890",
			Address = "123 Test St",
			PickupAddress = "456 Pickup St",
			DateOfBirth = DateTime.Now.AddYears(-25),
			Availabilities = new List<InstructorAvailability>
			{
				new InstructorAvailability { Status = "Available", Start = DateTime.Now, End = DateTime.Now.AddHours(1) }
			}
		};

		// Act & Assert
		Assert.NotNull(userProfile.Availabilities);
		Assert.Single(userProfile.Availabilities);
		Assert.Equal("Available", userProfile.Availabilities.First().Status);
	}
}
