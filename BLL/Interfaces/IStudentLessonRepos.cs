using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
	public interface IStudentLessonRepos
	{
		Task<StudentLesson> BookLessonAsync(StudentLesson lesson);
		Task<List<StudentLesson>> GetLessonsByInstructorEmailAsync(string instructorEmail);

	}
}
