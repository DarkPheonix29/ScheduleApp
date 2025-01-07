using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.Interfaces;
using BLL.Models;

namespace BLL.Manager
{
	public class EventManager : IEventManager
	{
		private readonly IStudentLessonRepos _lessonRepos;
		private readonly IInstructorAvailabilityRepos _availabilityRepos;

		public EventManager(IStudentLessonRepos lessonRepos, IInstructorAvailabilityRepos availabilityRepos)
		{
			_lessonRepos = lessonRepos;
			_availabilityRepos = availabilityRepos;
		}

		public async Task<StudentLesson> BookLessonAsync(StudentLesson lesson)
		{
			return await _lessonRepos.BookLessonAsync(lesson);
		}

		public async Task<List<StudentLesson>> GetStudentLessonsAsync(string studentEmail)
		{
			return await _lessonRepos.GetLessonsByInstructorEmailAsync(studentEmail);
		}

		public async Task<List<InstructorAvailability>> GetInstructorAvailabilityAsync()
		{
			return await _availabilityRepos.GetAllAvailabilityAsync();
		}

		public async Task<InstructorAvailability> AddAvailabilityAsync(string instructoremail, DateTime start, DateTime end, string status)
		{
			return await _availabilityRepos.AddAvailabilityAsync(instructoremail, start, end, status);
		}

		public async Task<bool> UpdateAvailabilityStatusAsync(int id, string status)
		{
			return await _availabilityRepos.UpdateAvailabilityStatusAsync(id, status);
		}

		// New Method to Fetch All Instructor Availabilities
		public async Task<List<InstructorAvailability>> GetAllInstructorAvailabilitiesAsync()
		{
			return await _availabilityRepos.GetAllAvailabilityAsync();
		}
	}
}
