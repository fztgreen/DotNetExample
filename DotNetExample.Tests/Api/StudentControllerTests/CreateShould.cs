using DotNetExample.Api.Controllers;
using DotNetExample.Domain.Commands;
using DotNetExample.Domain.Entities;
using DotNetExample.Domain.Queries;

using Microsoft.AspNetCore.Mvc;

using NSubstitute;

namespace DotNetExample.Tests.Api.StudentControllerTests;

[TestClass]
public partial class CreateShould
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
    public async Task ReturnCreated()
    {
        // Arrange
        var student = new Student
        {
            Id = "123",
            Name = "John Doe",
            Gpa = 3.5,
            Grade = "A"
        };

        // Act
        var result = await _sut.Create(student);

        // Assert
        Assert.IsInstanceOfType(result, typeof(CreatedAtActionResult));
        var createdResult = (CreatedAtActionResult)result;
        Assert.AreEqual(nameof(StudentController.Get), createdResult.ActionName);
        Assert.AreEqual(student, createdResult.Value);
        await _createStudent.Received(1).ExecuteAsync(student);
    }
}
