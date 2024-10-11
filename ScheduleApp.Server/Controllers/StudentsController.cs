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
    [HttpPost]
    public IActionResult AddStudent([FromBody] Student student)
    {
        if (student == null)
        {
            return BadRequest("Student data is required.");
        }

        try
        {
            _studentService.AddStudent(student);
            return CreatedAtAction(nameof(GetStudents), new { id = student.Id }, student);
        }
        catch (Exception ex)
        {
            // Log the exception (ex) if necessary
            return StatusCode(500, "Internal server error");
        }
    }

}
