using BLL.Models;
using BLL.Interfaces;
using Microsoft.EntityFrameworkCore;
using DAL;

namespace DAL.Services;

public class StudentService(ApplicationDbContext context) : IStudentService
{
    public IEnumerable<Student> GetAllStudents()
    {
        return context.Students.ToList(); // This will now work since _context is of type ApplicationDbContext
    }

    public void AddStudent(Student student)
    {
        context.Students.Add(student);
        context.SaveChanges();
    }
}
