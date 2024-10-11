using Microsoft.AspNetCore.Mvc;
using BLL.Interfaces;
using BLL.Models;
using System.Collections.Generic;

[ApiController]
[Route("api/[controller]")]
public class StudentsController : ControllerBase
{
    private readonly IStudentService _studentRepository;

    public StudentsController(IStudentService studentRepository)
    {
        _studentRepository = studentRepository;
    }

    // GET: api/students
    [HttpGet]
    public ActionResult<IEnumerable<Student>> GetStudents()
    {
        var students = _studentRepository.GetAllStudents();
        return Ok(students);
    }
}
