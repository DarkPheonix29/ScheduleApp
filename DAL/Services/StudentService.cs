using BLL.Interfaces;
using BLL.Models;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Services
{
    public class StudentService : IStudentService
    {
        private readonly ApplicationDbContext _context;

        public StudentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Student> GetAllStudents()
        {
            return _context.Students.ToList();
        }
    }
}
