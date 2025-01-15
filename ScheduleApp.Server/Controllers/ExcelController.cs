using BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class ExcelController : ControllerBase
{
	private readonly IExcelRepos _excelRepos;

	public ExcelController(IExcelRepos excelRepos)
	{
		_excelRepos = excelRepos;
	}

	[HttpPost("updateLessonStatus")]
	public async Task<IActionResult> UpdateLessonStatus([FromBody] UpdateLessonStatusRequest request)
	{
		try
		{
			await _excelRepos.UpdateLessonStatusAsync(request.ProfileId, request.LessonColumn, request.Status, request.Category);
			return Ok(new { message = "Lesson status updated successfully." });
		}
		catch (Exception ex)
		{
			return BadRequest(new { message = ex.Message });
		}
	}


	[HttpGet("getExcelData/{profileId}")]
	public async Task<IActionResult> GetExcelData(int profileId)
	{
		try
		{
			var excelData = await _excelRepos.GetExcelDataAsync(profileId);
			return Ok(excelData);
		}
		catch (Exception ex)
		{
			return BadRequest(new { message = ex.Message });
		}
	}

	[HttpPost("initializeExcelData")]
	public async Task<IActionResult> InitializeExcelData(int profileId, string topic, string subtopic, string category)
	{
		try
		{
			var data = new List<(string Category, string Topic, string Subtopic)>
		{
			(category, topic, subtopic)
		};

			await _excelRepos.InitializeExcelDataForProfileAsync(profileId, data);
			return Ok(new { message = "Excel data initialized successfully." });
		}
		catch (Exception ex)
		{
			return BadRequest(new { message = ex.Message });
		}
	}

}

public class UpdateLessonStatusRequest
{
	public int ProfileId { get; set; }
	public string LessonColumn { get; set; }
	public string Status { get; set; }
	public string Category { get; set; }
}

