using BLL.Models;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
	public interface IProfileRepos
	{
		Task<UserProfile> CreateUserProfileAsync(string email, string displayName, string phoneNumber, string address, string pickupAddress, DateTime dateOfBirth);
		Task<List<EmailData>> GetStudentEmailsAsync();
		Task<UserProfile> GetStudentProfileByEmailAsync(string email);
	}
}
