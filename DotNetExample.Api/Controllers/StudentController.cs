using DotNetExample.Domain.Commands;
using DotNetExample.Domain.Entities;
using DotNetExample.Domain.Queries;

using Microsoft.AspNetCore.Mvc;

namespace DotNetExample.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StudentController : ControllerBase
{
    private readonly ICreateStudent _createStudent;
    private readonly IGetStudent _getStudent;
    private readonly IGetAllStudents _getAllStudents;
    private readonly IUpdateStudent _updateStudent;
    private readonly IDeleteStudent _deleteStudent;

    public StudentController(
        ICreateStudent createStudent,
        IGetStudent getStudent,
        IGetAllStudents getAllStudents,
        IUpdateStudent updateStudent,
        IDeleteStudent deleteStudent)
    {
        _createStudent = createStudent;
        _getStudent = getStudent;
        _getAllStudents = getAllStudents;
        _updateStudent = updateStudent;
        _deleteStudent = deleteStudent;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Student student)
    {
        await _createStudent.ExecuteAsync(student);
        return CreatedAtAction(nameof(Get), new { id = student.Id }, student);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(string id)
    {
        var student = await _getStudent.ExecuteAsync(id);
        if (student == null)
        {
            return NotFound();
        }
        return Ok(student);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var students = await _getAllStudents.ExecuteAsync();
        return Ok(students);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] Student student)
    {
        var existingStudent = await _getStudent.ExecuteAsync(id);
        if (existingStudent == null)
        {
            return NotFound();
        }
        
        student.Id = id;
        await _updateStudent.ExecuteAsync(student);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var existingStudent = await _getStudent.ExecuteAsync(id);
        if (existingStudent == null)
        {
            return NotFound();
        }
        
        await _deleteStudent.ExecuteAsync(id);
        return NoContent();
    }
}
