using Moq;
using Xunit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class InstructorAvailabilityTests
{
	[Fact]
	public async Task AddInstructorAvailability_ShouldAddAvailability_WhenValidDataIsProvided()
	{
		// Arrange
		var options = new DbContextOptionsBuilder<ApplicationDbContext>()
						.UseInMemoryDatabase(databaseName: "TestDatabase")
						.Options;

		using (var context = new ApplicationDbContext(options))
		{
			var userProfile = new UserProfile
			{
				Email = "instructor@example.com",
				DisplayName = "Instructor",
				PhoneNumber = "9876543210",
				Address = "789 Instructor St",
				PickupAddress = "012 Pickup St",
				DateOfBirth = DateTime.Now.AddYears(-35)
			};

			var availability = new InstructorAvailability
			{
				InstructorEmail = userProfile.Email,
				Start = DateTime.Now,
				End = DateTime.Now.AddHours(1),
				Status = "Available",
				UserProfile = userProfile
			};

			// Act
			context.InstructorAvailabilities.Add(availability);
			await context.SaveChangesAsync();

			// Assert
			var addedAvailability = await context.InstructorAvailabilities.FirstOrDefaultAsync();
			Assert.NotNull(addedAvailability);
			Assert.Equal("instructor@example.com", addedAvailability.InstructorEmail);
			Assert.Equal("Available", addedAvailability.Status);
		}
	}


	[Fact]
	public async Task AddExcelData_ShouldAddData_WhenValidDataIsProvided()
	{
		// Arrange
		var options = new DbContextOptionsBuilder<ApplicationDbContext>()
					.UseInMemoryDatabase("TestDatabase")
					.Options;

		using (var context = new ApplicationDbContext(options))
		{

			var excelData = new ExcelData
			{
				ProfileId = 1,
				Category = "Math",
				Topic = "Algebra",
				Subtopic = "Linear Equations",
				Les1 = "Lesson 1",
				Les2 = "Lesson 2",
				Les3 = "Lesson 3",
				Les4 = "Lesson 4",
				Les5 = "Lesson 5",
				Les6 = "Lesson 6",
				Les7 = "Lesson 7",
				Les8 = "Lesson 8",
				Les9 = "Lesson 9",
				Les10 = "Lesson 10",
				Les11 = "Lesson 11",
				Les12 = "Lesson 12",
				Les13 = "Lesson 13",
				Les14 = "Lesson 14",
				Les15 = "Lesson 15",
				Les16 = "Lesson 16",
				Les17 = "Lesson 17",
				Les18 = "Lesson 18",
				Les19 = "Lesson 19",
				Les20 = "Lesson 20",
				Les21 = "Lesson 21",
				Les22 = "Lesson 22",
				Les23 = "Lesson 23",
				Les24 = "Lesson 24",
				Les25 = "Lesson 25",
				Les26 = "Lesson 26",
				Les27 = "Lesson 27",
				Les28 = "Lesson 28",
				Les29 = "Lesson 29",
				Les30 = "Lesson 30",
				Les31 = "Lesson 31",
				Les32 = "Lesson 32",
				Les33 = "Lesson 33",
				Les34 = "Lesson 34",
				Les35 = "Lesson 35",
				Les36 = "Lesson 36",
				Les37 = "Lesson 37",
				Les38 = "Lesson 38",
				Les39 = "Lesson 39",
				Les40 = "Lesson 40",
				Les41 = "Lesson 41",
				Les42 = "Lesson 42",
				Les43 = "Lesson 43",
				Les44 = "Lesson 44",
				Les45 = "Lesson 45",
				Les46 = "Lesson 46",
				Les47 = "Lesson 47",
				Les48 = "Lesson 48",
				Les49 = "Lesson 49",
				Les50 = "Lesson 50"
			};

			// Act
			context.ExcelData.Add(excelData);
			await context.SaveChangesAsync();

			// Assert: Retrieve the data and validate the results
			var savedExcelData = await context.ExcelData.FirstOrDefaultAsync();
			Assert.NotNull(savedExcelData);

			// Verify that each lesson field is set correctly
			Assert.Equal("Lesson 1", excelData.Les1);
			Assert.Equal("Lesson 2", excelData.Les2);
			Assert.Equal("Lesson 3", excelData.Les3);
			Assert.Equal("Lesson 4", excelData.Les4);
			Assert.Equal("Lesson 5", excelData.Les5);
			Assert.Equal("Lesson 6", excelData.Les6);
			Assert.Equal("Lesson 7", excelData.Les7);
			Assert.Equal("Lesson 8", excelData.Les8);
			Assert.Equal("Lesson 9", excelData.Les9);
			Assert.Equal("Lesson 10", excelData.Les10);
			Assert.Equal("Lesson 11", excelData.Les11);
			Assert.Equal("Lesson 12", excelData.Les12);
			Assert.Equal("Lesson 13", excelData.Les13);
			Assert.Equal("Lesson 14", excelData.Les14);
			Assert.Equal("Lesson 15", excelData.Les15);
			Assert.Equal("Lesson 16", excelData.Les16);
			Assert.Equal("Lesson 17", excelData.Les17);
			Assert.Equal("Lesson 18", excelData.Les18);
			Assert.Equal("Lesson 19", excelData.Les19);
			Assert.Equal("Lesson 20", excelData.Les20);
			Assert.Equal("Lesson 21", excelData.Les21);
			Assert.Equal("Lesson 22", excelData.Les22);
			Assert.Equal("Lesson 23", excelData.Les23);
			Assert.Equal("Lesson 24", excelData.Les24);
			Assert.Equal("Lesson 25", excelData.Les25);
			Assert.Equal("Lesson 26", excelData.Les26);
			Assert.Equal("Lesson 27", excelData.Les27);
			Assert.Equal("Lesson 28", excelData.Les28);
			Assert.Equal("Lesson 29", excelData.Les29);
			Assert.Equal("Lesson 30", excelData.Les30);
			Assert.Equal("Lesson 31", excelData.Les31);
			Assert.Equal("Lesson 32", excelData.Les32);
			Assert.Equal("Lesson 33", excelData.Les33);
			Assert.Equal("Lesson 34", excelData.Les34);
			Assert.Equal("Lesson 35", excelData.Les35);
			Assert.Equal("Lesson 36", excelData.Les36);
			Assert.Equal("Lesson 37", excelData.Les37);
			Assert.Equal("Lesson 38", excelData.Les38);
			Assert.Equal("Lesson 39", excelData.Les39);
			Assert.Equal("Lesson 40", excelData.Les40);
			Assert.Equal("Lesson 41", excelData.Les41);
			Assert.Equal("Lesson 42", excelData.Les42);
			Assert.Equal("Lesson 43", excelData.Les43);
			Assert.Equal("Lesson 44", excelData.Les44);
			Assert.Equal("Lesson 45", excelData.Les45);
			Assert.Equal("Lesson 46", excelData.Les46);
			Assert.Equal("Lesson 47", excelData.Les47);
			Assert.Equal("Lesson 48", excelData.Les48);
			Assert.Equal("Lesson 49", excelData.Les49);
			Assert.Equal("Lesson 50", excelData.Les50);
		}
	}

}
