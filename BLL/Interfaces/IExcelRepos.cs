using BLL.Models;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
	public interface IExcelRepos
	{
		Task UpdateLessonStatusAsync(int profileId, string lessonColumn, string status, string category);
		Task<List<ExcelData>> GetExcelDataAsync(int profileId);
		Task InitializeExcelDataForProfileAsync(int profileId, List<(string Category, string Topic, string Subtopic)> data);
	}

}
