using BLL.Models;
using BLL.Interfaces;

namespace DAL.Services;

public class StudentService(ApplicationDbContext context) : IStudentService
{
    public IEnumerable<Student> GetAllStudents()
    {
        return context.Students.ToList();
    }

    public void AddStudent(Student student)
    {
        context.Students.Add(student);
        context.SaveChanges();
    }
}
