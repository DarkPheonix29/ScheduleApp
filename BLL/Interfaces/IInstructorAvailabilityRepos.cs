using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
	public interface IInstructorAvailabilityRepos
	{
		Task<List<InstructorAvailability>> GetAllAvailabilityAsync();
		Task<InstructorAvailability> AddAvailabilityAsync(string instructoremail, DateTime start, DateTime end, string status);
		Task<bool> UpdateAvailabilityStatusAsync(int id, string status);
	}
}
