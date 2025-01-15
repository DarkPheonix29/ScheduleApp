using BLL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

public class ExcelRepos : IExcelRepos
{
	private readonly ApplicationDbContext _dbContext;

	public ExcelRepos(ApplicationDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public async Task UpdateLessonStatusAsync(int profileId, string lessonColumn, string status, string category)
	{
		try
		{
			// Validate the status
			if (!new[] { "-", "x", "h", "o", "v" }.Contains(status))
			{
				throw new ArgumentException("Invalid status value.");
			}

			// Retrieve the record for the specific profile and category
			var excelData = await _dbContext.ExcelData
				.FirstOrDefaultAsync(e => e.ProfileId == profileId && e.Category == category);

			if (excelData == null)
			{
				throw new Exception("Excel data not found for the given profile and category.");
			}

			// Use reflection to dynamically update the lesson column (e.g., Les1, Les2, etc.)
			var property = typeof(ExcelData).GetProperty(lessonColumn, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

			if (property == null || property.PropertyType != typeof(string))
			{
				throw new ArgumentException("Invalid lesson column name.");
			}

			property.SetValue(excelData, status); // Set the status for the correct lesson column

			// Save changes
			_dbContext.ExcelData.Update(excelData);
			await _dbContext.SaveChangesAsync();
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Error updating lesson status: {ex.Message}");
			throw;
		}
	}

	public async Task<List<ExcelData>> GetExcelDataAsync(int profileId)
	{
		try
		{
			// Fetch all rows matching the profileId
			return await _dbContext.ExcelData
								   .Where(e => e.ProfileId == profileId)
								   .ToListAsync();
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Error retrieving Excel data: {ex.Message}");
			throw;
		}
	}


	public async Task InitializeExcelDataForProfileAsync(int profileId, List<(string Category, string Topic, string Subtopic)> data)
	{
		Console.WriteLine("Initializing Excel data for profile...");
		try
		{
			// Check if data already exists for the profile
			var existingData = await _dbContext.ExcelData.FirstOrDefaultAsync(e => e.ProfileId == profileId);

			if (existingData != null)
			{
				throw new Exception("Excel data already exists for this profile.");
			}
			Console.WriteLine("Excel data does not exist for this profile. Proceeding with initialization...");

			// Create ExcelData entries for each row in the provided data
			var excelDataEntries = data.Select(d => new ExcelData
			{
				ProfileId = profileId,
				Category = d.Category,
				Topic = d.Topic,
				Subtopic = d.Subtopic,
				Les1 = null,
				Les2 = null,
				Les3 = null,
				Les4 = null,
				Les5 = null,
				Les6 = null,
				Les7 = null,
				Les8 = null,
				Les9 = null,
				Les10 = null,
				Les11 = null,
				Les12 = null,
				Les13 = null,
				Les14 = null,
				Les15 = null,
				Les16 = null,
				Les17 = null,
				Les18 = null,
				Les19 = null,
				Les20 = null,
				Les21 = null,
				Les22 = null,
				Les23 = null,
				Les24 = null,
				Les25 = null,
				Les26 = null,
				Les27 = null,
				Les28 = null,
				Les29 = null,
				Les30 = null,
				Les31 = null,
				Les32 = null,
				Les33 = null,
				Les34 = null,
				Les35 = null,
				Les36 = null,
				Les37 = null,
				Les38 = null,
				Les39 = null,
				Les40 = null,
				Les41 = null,
				Les42 = null,
				Les43 = null,
				Les44 = null,
				Les45 = null,
				Les46 = null,
				Les47 = null,
				Les48 = null,
				Les49 = null,
				Les50 = null
			}).ToList();

			// Add entries to the database
			await _dbContext.ExcelData.AddRangeAsync(excelDataEntries);
			await _dbContext.SaveChangesAsync();

			Console.WriteLine("Excel data initialized successfully.");
		}
		catch (DbUpdateException ex)
		{
			Console.WriteLine($"Error initializing Excel data: {ex.Message}");
			throw;
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Error initializing Excel data: {ex.Message}");
			throw;
		}
	}

	public async Task<List<ExcelData>> GetAllExcelDataForProfileAsync(int profileId)
	{
		try
		{
			return await _dbContext.ExcelData.Where(e => e.ProfileId == profileId).ToListAsync();
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Error retrieving all Excel data for profile: {ex.Message}");
			throw;
		}
	}
}
