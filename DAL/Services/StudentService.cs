using BLL.Models;
using BLL.Interfaces;
using Microsoft.EntityFrameworkCore;
using DAL;

namespace DAL.Services;
public class StudentService : IStudentService
{
    private readonly ApplicationDbContext _context; // Change DbContext to ApplicationDbContext

    public StudentService(ApplicationDbContext context) // Change parameter type to ApplicationDbContext
    {
        _context = context;
    }

    public IEnumerable<Student> GetAllStudents()
    {
        return _context.Students.ToList(); // This will now work since _context is of type ApplicationDbContext
    }

    public void AddStudent(Student student)
    {
        _context.Students.Add(student);
        _context.SaveChanges();
    }
}
