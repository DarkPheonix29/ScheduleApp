using BLL.Interfaces;
using BLL.Models;
using DAL;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DAL.repos
{
	public class ProfileRepos : IProfileRepos
	{
		private readonly ApplicationDbContext _dbContext;

		public ProfileRepos(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<UserProfile> CreateUserProfileAsync(string email, string displayName, string phoneNumber, string address, string pickupAddress, DateTime dateOfBirth)
		{
			var userProfile = new UserProfile
			{
				Email = email,
				DisplayName = displayName,
				PhoneNumber = phoneNumber,
				Address = address,
				PickupAddress = pickupAddress,
				DateOfBirth = dateOfBirth
			};

			// Save the user profile to the database
			_dbContext.UserProfiles.Add(userProfile);
			await _dbContext.SaveChangesAsync();

			return userProfile;
		}
	}
}
