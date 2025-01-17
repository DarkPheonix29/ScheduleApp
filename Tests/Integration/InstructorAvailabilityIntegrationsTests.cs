using BLL.Interfaces;
using BLL.Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using ScheduleApp.Server.Controllers;
using BLL.Manager; // Replace with the actual namespace for your controller

namespace Tests.Unit
{
	public class InstructorAvailabilityControllerTests
	{
		private readonly Mock<IEventManager> _mockEventManager;
		private readonly InstructorAvailabilityController _controller;

		public InstructorAvailabilityControllerTests()
		{
			_mockEventManager = new Mock<IEventManager>();
			_controller = new InstructorAvailabilityController(_mockEventManager.Object);
		}

		// Test AddAvailability (Happy Path)
		[Fact]
		public async Task AddAvailability_ShouldReturnOk_WhenAvailabilityIsAddedSuccessfully()
		{
			// Arrange
			var availability = new AddAvailability
			{
				InstructorEmail = "instructor@example.com",
				Start = DateTime.UtcNow.AddDays(1),
				End = DateTime.UtcNow.AddDays(1).AddHours(2),
				Status = "Available"
			};

			_mockEventManager.Setup(manager => manager.AddAvailabilityAsync(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<string>()))
				.ReturnsAsync(new InstructorAvailability
				{
					InstructorEmail = availability.InstructorEmail,
					Start = availability.Start,
					End = availability.End,
					Status = availability.Status
				});

			// Act
			var result = await _controller.AddAvailability(availability);

			// Assert
			var okResult = Assert.IsType<OkObjectResult>(result);
			var response = okResult.Value;
			Assert.NotNull(response);
			Assert.Equal("Availability added successfully.", response.GetType().GetProperty("message")?.GetValue(response, null));
		}

		// Test AddAvailability (Error Path - Exception Handling)
		[Fact]
		public async Task AddAvailability_ShouldReturnBadRequest_WhenAnExceptionOccurs()
		{
			// Arrange
			var availability = new AddAvailability
			{
				InstructorEmail = "instructor@example.com",
				Start = DateTime.UtcNow.AddDays(1),
				End = DateTime.UtcNow.AddDays(1).AddHours(2),
				Status = "Available"
			};

			_mockEventManager.Setup(manager => manager.AddAvailabilityAsync(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<string>()))
				.ThrowsAsync(new Exception("Error occurred while adding availability"));

			// Act
			var result = await _controller.AddAvailability(availability);

			// Assert
			var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
			var response = badRequestResult.Value;
			Assert.NotNull(response);
			Assert.Equal("Error occurred while adding availability", response.GetType().GetProperty("message")?.GetValue(response, null));
		}

		// Test AddAvailability (Validation: InstructorEmail Missing)
		[Fact]
		public async Task AddAvailability_ShouldReturnBadRequest_WhenInstructorEmailIsMissing()
		{
			// Arrange
			var availability = new AddAvailability
			{
				InstructorEmail = "",
				Start = DateTime.UtcNow.AddDays(1),
				End = DateTime.UtcNow.AddDays(1).AddHours(2),
				Status = "Available"
			};

			// Act
			var result = await _controller.AddAvailability(availability);

			// Assert
			var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
			var response = badRequestResult.Value;
			Assert.NotNull(response);
			Assert.Equal("Instructor email is required", response.GetType().GetProperty("message")?.GetValue(response, null));
		}
	}
}
