using BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class ExcelController : ControllerBase
{
	private readonly IExcelRepos _excelRepos;

	public ExcelController(IExcelRepos excelRepos)
	{
		_excelRepos = excelRepos;
	}

	[HttpGet("excelget/{email}")]
	public async Task<IActionResult> GetInstructorCard(string email)
	{
		try
		{
			var fileBytes = await _excelRepos.GetInstructorCardAsync(email);

			if (fileBytes == null || fileBytes.Length == 0)
			{
				return NotFound(new { message = "Instructor card not found." });
			}

			return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Instructeurkaarttemplate.xlsx");
		}
		catch (Exception ex)
		{
			return BadRequest(new { message = ex.Message });
		}
	}

	[HttpPost("excelpost/{email}")]
	public async Task<IActionResult> SaveInstructorCard(string email, [FromBody] InstructorCardFormData formData)
	{
		try
		{
			if (formData == null || formData.FileBytes == null || formData.FileBytes.Length == 0)
			{
				return BadRequest(new { message = "Invalid file data." });
			}

			await _excelRepos.SaveInstructorCardAsync(email, formData.FileBytes);

			return Ok(new { message = "File saved successfully." });
		}
		catch (Exception ex)
		{
			return BadRequest(new { message = ex.Message });
		}
	}

	public class InstructorCardFormData
	{
		public byte[] FileBytes { get; set; }
	}
}
