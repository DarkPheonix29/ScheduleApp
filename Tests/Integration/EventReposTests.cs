using Microsoft.EntityFrameworkCore;
using DAL.Repos;
using BLL.Models;
using Xunit;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using DAL;


namespace Tests.Integration
{
	public class EventReposTests
	{
		private readonly ApplicationDbContext _context;
		private readonly EventRepos _repo;

		public EventReposTests()
		{
			// Setup in-memory database
			var options = new DbContextOptionsBuilder<ApplicationDbContext>()
				.UseInMemoryDatabase(databaseName: "TestDatabase")
				.Options;

			_context = new ApplicationDbContext(options);
			_repo = new EventRepos(_context);
			
			_context.Database.EnsureDeleted();
			_context.Database.EnsureCreated();
			// Seed database with initial data
			SeedDatabase();
		}

		private void SeedDatabase()
		{
			
			_context.Events.AddRange(new List<Event>
{
	new Event { Title = "Event 1", InstructorId = 1, Status = "Available", Start = DateTime.Now.AddHours(2), End = DateTime.Now.AddHours(3), StudentId = null },
	new Event { Title = "Event 2", InstructorId = 1, Status = "Available", Start = DateTime.Now.AddDays(1), End = DateTime.Now.AddDays(1).AddHours(1), StudentId = null }
});

			_context.SaveChanges();

		}


		[Fact]
		public async Task GetAllEventsAsync_ReturnsEvents()
		{
			// Act
			var events = await _repo.GetAllEventsAsync();

			// Assert
			Assert.NotNull(events);
			Assert.Equal(2, events.Count); // Should match the seeded events
		}

		[Fact]
		public async Task GetEventsByInstructorIdAsync_ReturnsEventsForInstructor()
		{
			// Act
			var events = await _repo.GetEventsByInstructorIdAsync(1);

			// Assert
			Assert.NotNull(events);
			Assert.Equal(2, events.Count); // All seeded events belong to InstructorId = 1
		}

		[Fact]
		public async Task AddEventAsync_AddsNewEvent()
		{
			// Arrange
			var newEvent = new Event
			{
				Title = "New Event",
				InstructorId = 1,
				Status = "Available",
				Start = DateTime.Now.AddDays(2),
				End = DateTime.Now.AddDays(2).AddHours(1),
				StudentId = null
			};

			// Act
			var addedEvent = await _repo.AddEventAsync(newEvent);

			// Assert
			Assert.NotNull(addedEvent);
			Assert.Equal("New Event", addedEvent.Title);

			// Check if the new event is in the database
			var eventInDb = await _context.Events.FindAsync(addedEvent.Id);
			Assert.NotNull(eventInDb);
		}

		[Fact]
		public async Task DeleteEventAsync_DeletesEvent()
		{
			// Arrange
			var eventToDelete = await _context.Events.FirstOrDefaultAsync(e => e.Id == 1);
			Assert.NotNull(eventToDelete);

			// Act
			var result = await _repo.DeleteEventAsync(eventToDelete.Id);

			// Assert
			Assert.True(result); // The deletion should be successful
			var deletedEvent = await _context.Events.FindAsync(eventToDelete.Id);
			Assert.Null(deletedEvent); // The event should be deleted
		}

		[Fact]
		public async Task BookLessonAsync_BooksAvailableEvent()
		{
			// Arrange
			var eventToBook = await _context.Events.FirstOrDefaultAsync(e => e.Status == "Available");
			Assert.NotNull(eventToBook);

			// Act
			var bookedEvent = await _repo.BookLessonAsync(eventToBook.Id, 1);

			// Assert
			Assert.NotNull(bookedEvent);
			Assert.Equal("Booked", bookedEvent.Status); // Event status should be updated
		}

		[Fact]
		public async Task CheckAvailabilityAsync_ReturnsTrueIfAvailable()
		{
			// Act
			var availability = await _repo.CheckAvailabilityAsync(1, DateTime.Now, DateTime.Now.AddHours(1));

			// Assert
			Assert.True(availability); // Should return true as no events should overlap
		}
	}
}
