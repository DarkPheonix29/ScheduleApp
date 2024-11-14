using Microsoft.AspNetCore.Mvc;
using BLL.Interfaces;
using BLL.Models;

[ApiController]
[Route("api/[controller]")]
public class StudentsController : ControllerBase
{
    private readonly IStudentRepos _studentService;

    public StudentsController(IStudentRepos studentService)
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
    public IActionResult AddStudent(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return BadRequest("Student data is required.");
        }

        try
        {
            _studentService.AddStudent(new Student(name));
            return Created();
        }
        catch (Exception ex)
        {
            // Log the exception (ex) if necessary
            return StatusCode(500, "Internal server error");
        }
    }

}
