using BLL.Interfaces;
using Moq;
using Xunit;
using System;
using BLL.Manager;

namespace Tests.Unit
{
	public class EventManagerTests
	{
		private readonly Mock<IStudentLessonRepos> _mockLessonRepos;
		private readonly Mock<IInstructorAvailabilityRepos> _mockAvailabilityRepos;
		private readonly EventManager _eventManager;

		public EventManagerTests()
		{
			_mockLessonRepos = new Mock<IStudentLessonRepos>();
			_mockAvailabilityRepos = new Mock<IInstructorAvailabilityRepos>();
			_eventManager = new EventManager(_mockLessonRepos.Object, _mockAvailabilityRepos.Object);
		}

		[Fact]
		public async Task BookLessonAsync_CallsLessonReposAndBooksLesson()
		{
			var lesson = new StudentLesson { StudentEmail = "student@example.com", InstructorEmail = "instructor@example.com" };
			_mockLessonRepos.Setup(x => x.BookLessonAsync(lesson)).ReturnsAsync(lesson);

			var result = await _eventManager.BookLessonAsync(lesson);

			_mockLessonRepos.Verify(x => x.BookLessonAsync(lesson), Times.Once);
			Assert.Equal(lesson, result);
		}

		[Fact]
		public async Task BookLessonAsync_Failure_ThrowsException()
		{
			var lesson = new StudentLesson { StudentEmail = "student@example.com", InstructorEmail = "instructor@example.com" };
			_mockLessonRepos.Setup(x => x.BookLessonAsync(lesson)).ThrowsAsync(new Exception("Booking failed"));

			await Assert.ThrowsAsync<Exception>(() => _eventManager.BookLessonAsync(lesson));
		}

		[Fact]
		public async Task AddAvailabilityAsync_CallsAvailabilityReposAndAddsAvailability()
		{
			var availability = new InstructorAvailability { InstructorEmail = "instructor@example.com", Start = DateTime.Now, End = DateTime.Now.AddHours(1) };
			_mockAvailabilityRepos.Setup(x => x.AddAvailabilityAsync(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<string>())).ReturnsAsync(availability);

			var result = await _eventManager.AddAvailabilityAsync("instructor@example.com", DateTime.Now, DateTime.Now.AddHours(1), "available");

			_mockAvailabilityRepos.Verify(x => x.AddAvailabilityAsync(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<string>()), Times.Once);
			Assert.Equal(availability, result);
		}

		[Fact]
		public async Task AddAvailabilityAsync_InvalidData_ThrowsException()
		{
			_mockAvailabilityRepos.Setup(x => x.AddAvailabilityAsync(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<string>())).ThrowsAsync(new Exception("Invalid availability"));

			await Assert.ThrowsAsync<Exception>(() => _eventManager.AddAvailabilityAsync("instructor@example.com", DateTime.Now, DateTime.Now.AddHours(1), "invalid"));
		}

		[Fact]
		public async Task UpdateAvailabilityStatusAsync_CallsReposAndUpdatesStatus()
		{
			int availabilityId = 1;
			string status = "confirmed";
			_mockAvailabilityRepos.Setup(x => x.UpdateAvailabilityStatusAsync(availabilityId, status)).ReturnsAsync(true);

			var result = await _eventManager.UpdateAvailabilityStatusAsync(availabilityId, status);

			_mockAvailabilityRepos.Verify(x => x.UpdateAvailabilityStatusAsync(availabilityId, status), Times.Once);
			Assert.True(result);
		}

		[Fact]
		public async Task GetStudentLessonsAsync_CallsLessonReposAndReturnsLessons()
		{
			string studentEmail = "student@example.com";
			var lessons = new List<StudentLesson> { new StudentLesson { StudentEmail = studentEmail } };
			_mockLessonRepos.Setup(x => x.GetLessonsByInstructorEmailAsync(studentEmail)).ReturnsAsync(lessons);

			var result = await _eventManager.GetStudentLessonsAsync(studentEmail);

			_mockLessonRepos.Verify(x => x.GetLessonsByInstructorEmailAsync(studentEmail), Times.Once);
			Assert.Equal(lessons.Count, result.Count);
		}

		[Fact]
		public async Task GetInstructorAvailabilityAsync_CallsAvailabilityReposAndReturnsAvailability()
		{
			var availability = new List<InstructorAvailability> { new InstructorAvailability { InstructorEmail = "instructor@example.com" } };
			_mockAvailabilityRepos.Setup(x => x.GetAllAvailabilityAsync()).ReturnsAsync(availability);

			var result = await _eventManager.GetInstructorAvailabilityAsync();

			_mockAvailabilityRepos.Verify(x => x.GetAllAvailabilityAsync(), Times.Once);
			Assert.Equal(availability.Count, result.Count);
		}

		[Fact]
		public async Task GetInstructorAvailabilityAsync_Failure_ThrowsException()
		{
			_mockAvailabilityRepos.Setup(x => x.GetAllAvailabilityAsync()).ThrowsAsync(new Exception("Failed to fetch availability"));

			await Assert.ThrowsAsync<Exception>(() => _eventManager.GetInstructorAvailabilityAsync());
		}

		[Fact]
		public async Task GetAllInstructorAvailabilitiesAsync_CallsAvailabilityReposAndReturnsAvailability()
		{
			var availability = new List<InstructorAvailability> { new InstructorAvailability { InstructorEmail = "instructor@example.com" } };
			_mockAvailabilityRepos.Setup(x => x.GetAllAvailabilityAsync()).ReturnsAsync(availability);

			var result = await _eventManager.GetAllInstructorAvailabilitiesAsync();

			Assert.Equal(availability.Count, result.Count);
		}
	}
}
