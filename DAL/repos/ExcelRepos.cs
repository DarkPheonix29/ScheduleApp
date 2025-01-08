using BLL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

public class ExcelRepos : IExcelRepos
{
	private readonly ApplicationDbContext _dbContext;
	private readonly IConfiguration _configuration;
	public ExcelRepos(ApplicationDbContext dbContext, IConfiguration configuration)
	{
		_dbContext = dbContext;
		_configuration = configuration;
	}

	public async Task SaveInstructorCardAsync(string email, byte[] fileBytes)
	{
		try
		{
			var userProfile = await _dbContext.UserProfiles.FirstOrDefaultAsync(up => up.Email == email);
			if (userProfile == null)
			{
				throw new Exception("User profile not found.");
			}

			userProfile.InstructorCard = fileBytes;

			_dbContext.UserProfiles.Update(userProfile);
			await _dbContext.SaveChangesAsync();

			Console.WriteLine($"Instructor card saved for user: {email}");

			// Verify that the file was saved correctly
			var savedFileBytes = await GetInstructorCardAsync(email);
			if (savedFileBytes == null || savedFileBytes.Length == 0)
			{
				throw new Exception("Failed to save the instructor card to the database.");
			}
		}
		catch (DbUpdateException ex)
		{
			Console.WriteLine($"Error saving instructor card: {ex.Message}");
			throw;
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Error saving instructor card: {ex.Message}");
			throw;
		}
	}

	// New method to handle saving during signup
	public async Task SaveInstructorCardDuringSignupAsync(string email)
	{
		try
		{
			// Path to the Excel template file
			var templatePath = _configuration["ExcelTemplate:ExcelTemplatePath"];

			// Ensure the template file exists
			if (!System.IO.File.Exists(templatePath))
			{
				throw new FileNotFoundException($"Template file not found at: {templatePath}");
			}

			// Read the template file into a byte array
			var fileBytes = System.IO.File.ReadAllBytes(templatePath);

			Console.WriteLine($"Read {fileBytes.Length} bytes from template file.");

			// Use raw SQL to insert the file if InstructorCard is NULL
			var query = @"
            UPDATE UserProfiles
            SET InstructorCard = @InstructorCard
            WHERE Email = @Email AND InstructorCard IS NULL;
        ";

			// Execute the query using ExecuteSqlRawAsync to prevent SQL injection
			var rowsAffected = await _dbContext.Database.ExecuteSqlRawAsync(
				query,
				new Microsoft.Data.Sqlite.SqliteParameter("@InstructorCard", fileBytes),
				new Microsoft.Data.Sqlite.SqliteParameter("@Email", email)
			);

			// Log rows affected
			Console.WriteLine($"Rows affected by the update: {rowsAffected}");

			// Check if any rows were affected (i.e., the file was inserted)
			if (rowsAffected == 0)
			{
				Console.WriteLine($"Instructor card not updated or already set for: {email}");
			}
			else
			{
				Console.WriteLine($"Instructor card saved during signup for user: {email}");
			}

			// Directly query the InstructorCard field to verify if it's saved properly
			var userProfile = await _dbContext.UserProfiles
				.FirstOrDefaultAsync(up => up.Email == email);

			if (userProfile != null)
			{
				if (userProfile.InstructorCard == null || userProfile.InstructorCard.Length == 0)
				{
					Console.WriteLine("Instructor card exists but is empty.");
					throw new Exception("Instructor card is empty in the database.");
				}

				Console.WriteLine("Instructor card retrieved successfully.");
			}
			else
			{
				throw new Exception("User profile not found when verifying instructor card.");
			}
		}
		catch (DbUpdateException ex)
		{
			Console.WriteLine($"Error saving instructor card during signup: {ex.Message}");
			throw;
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Error saving instructor card during signup: {ex.Message}");
			throw;
		}
	}


	public async Task<byte[]> GetInstructorCardAsync(string email)
	{
		try
		{
			var userProfile = await _dbContext.UserProfiles.FirstOrDefaultAsync(up => up.Email == email);
			if (userProfile == null)
			{
				throw new Exception("User profile not found.");
			}

			return userProfile.InstructorCard;
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Error retrieving instructor card: {ex.Message}");
			throw;
		}
	}
}
