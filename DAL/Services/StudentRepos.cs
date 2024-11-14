using BLL.Models;
using BLL.Interfaces;

namespace DAL.Services;

public class StudentRepos(ApplicationDbContext context) : IStudentRepos
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
