using DotNetExample.Api.Controllers;
using DotNetExample.Domain.Commands;
using DotNetExample.Domain.Entities;
using DotNetExample.Domain.Queries;

using Microsoft.AspNetCore.Mvc;

using NSubstitute;

namespace DotNetExample.Tests.Api.StudentControllerTests;

[TestClass]
public partial class UpdateShould
{
    private ICreateStudent _createStudent;
    private IGetStudent _getStudent;
    private IGetAllStudents _getAllStudents;
    private IUpdateStudent _updateStudent;
    private IDeleteStudent _deleteStudent;
    private StudentController _sut;

    [TestInitialize]
    public void TestInitialize()
    {
        _createStudent = Substitute.For<ICreateStudent>();
        _getStudent = Substitute.For<IGetStudent>();
        _getAllStudents = Substitute.For<IGetAllStudents>();
        _updateStudent = Substitute.For<IUpdateStudent>();
        _deleteStudent = Substitute.For<IDeleteStudent>();
        _sut = new StudentController(_createStudent, _getStudent, _getAllStudents, _updateStudent, _deleteStudent);
    }

    [TestMethod]
    public async Task ReturnNoContent()
    {
        // Arrange
        var existingStudent = new Student { Id = "123", Name = "John Doe", Gpa = 3.5, Grade = "A" };
        var updatedStudent = new Student { Id = "123", Name = "John Updated", Gpa = 3.9, Grade = "A+" };
        _getStudent.ExecuteAsync("123").Returns(existingStudent);

        // Act
        var result = await _sut.Update("123", updatedStudent);

        // Assert
        Assert.IsInstanceOfType(result, typeof(NoContentResult));
        await _updateStudent.Received(1).ExecuteAsync(Arg.Is<Student>(s => s.Id == "123"));
    }

    [TestMethod]
    public async Task ReturnNotFound_WhenStudentDoesNotExist()
    {
        // Arrange
        var student = new Student { Id = "999", Name = "Non Existent", Gpa = 3.0, Grade = "B" };
        _getStudent.ExecuteAsync("999").Returns((Student?)null);

        // Act
        var result = await _sut.Update("999", student);

        // Assert
        Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        await _updateStudent.DidNotReceive().ExecuteAsync(Arg.Any<Student>());
    }
}
