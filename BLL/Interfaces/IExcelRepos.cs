using BLL.Models;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
	public interface IExcelRepos
	{
		Task SaveInstructorCardAsync(string email, byte[] fileBytes);
		Task<byte[]> GetInstructorCardAsync(string email);
		Task SaveInstructorCardDuringSignupAsync(string email);
	}

}
