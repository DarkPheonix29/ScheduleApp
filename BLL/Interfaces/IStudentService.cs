using BLL.Models;
using System.Collections.Generic;

namespace BLL.Interfaces
{
    public interface IStudentService
    {
        IEnumerable<Student> GetAllStudents();
    }
}
    
