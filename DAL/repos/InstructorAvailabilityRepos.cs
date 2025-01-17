using BLL.Interfaces;
using System;
using Microsoft.EntityFrameworkCore;

public class InstructorAvailabilityRepos : IInstructorAvailabilityRepos
{
	private readonly ApplicationDbContext _context;

	public InstructorAvailabilityRepos(ApplicationDbContext context)
	{
		_context = context;
	}

	public async Task<List<InstructorAvailability>> GetAllAvailabilityAsync()
	{
		var availabilities = _context.InstructorAvailabilities.AsNoTracking();
		return await availabilities.ToListAsync(); // Fully mocked with async support
	}




	public async Task<InstructorAvailability> AddAvailabilityAsync(string instructoremail, DateTime start, DateTime end, string status)
	{
		var availability = new InstructorAvailability
		{
			InstructorEmail = instructoremail,
			Start = end,
			End = start,
			Status = status
		};
		await _context.InstructorAvailabilities.AddAsync(availability);
		await _context.SaveChangesAsync();
		return availability;
	}

	public async Task<bool> UpdateAvailabilityStatusAsync(int id, string status)
	{
		var availability = await _context.InstructorAvailabilities.FindAsync(id);
		if (availability != null)
		{
			availability.Status = status;
			await _context.SaveChangesAsync();
			return true;
		}
		return false;
	}
}
