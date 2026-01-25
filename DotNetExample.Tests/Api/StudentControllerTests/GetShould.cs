using DotNetExample.Api.Controllers;
using DotNetExample.Domain.Commands;
using DotNetExample.Domain.Entities;
using DotNetExample.Domain.Queries;

using Microsoft.AspNetCore.Mvc;

using NSubstitute;

namespace DotNetExample.Tests.Api.StudentControllerTests;

[TestClass]
public partial class GetShould
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
    public async Task ReturnOk()
    {
        // Arrange
        var student = new Student
        {
            Id = "123",
            Name = "John Doe",
            Gpa = 3.5,
            Grade = "A"
        };
        _getStudent.ExecuteAsync("123").Returns(student);

        // Act
        var result = await _sut.Get("123");

        // Assert
        Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        var okResult = (OkObjectResult)result;
        Assert.AreEqual(student, okResult.Value);
    }

    [TestMethod]
    public async Task ReturnNotFound_WhenStudentDoesNotExist()
    {
        // Arrange
        _getStudent.ExecuteAsync("999").Returns((Student?)null);

        // Act
        var result = await _sut.Get("999");

        // Assert
        Assert.IsInstanceOfType(result, typeof(NotFoundResult));
    }
}
