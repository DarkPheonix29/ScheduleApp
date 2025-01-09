using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.Models;

namespace BLL.Manager
{
	public interface IEventManager
	{
		Task<StudentLesson> BookLessonAsync(StudentLesson lesson);
		Task<List<StudentLesson>> GetStudentLessonsAsync(string studentEmail);
		Task<List<InstructorAvailability>> GetInstructorAvailabilityAsync();
		Task<InstructorAvailability> AddAvailabilityAsync(string instructoremail, DateTime start, DateTime end, string status);
		Task<bool> UpdateAvailabilityStatusAsync(int id, string status);
	}
}
