using Microsoft.AspNetCore.Mvc;
using BLL.Interfaces;
using BLL.Models;
using System.Collections.Generic;

[ApiController]
[Route("api/[controller]")]
public class StudentsController : ControllerBase
{
    private readonly IStudentService _studentService;

    public StudentsController(IStudentService studentService)
    {
        _studentService = studentService;
    }

    // GET: api/students
    [HttpGet]
    public ActionResult<IEnumerable<Student>> GetStudents()
    {
        var students = _studentService.GetAllStudents();
        return Ok(students);
    }

    // POST: api/students
    [HttpPost]
    public ActionResult<Student> CreateStudent(Student student)
    {
        if (student == null)
        {
            return BadRequest("Student cannot be null.");
        }

        _studentService.AddStudent(student); // You will need to implement this method in IStudentService
        return CreatedAtAction(nameof(GetStudents), new { id = student.Id }, student);
    }
}
