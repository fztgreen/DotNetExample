using DotNetExample.Api.Controllers;
using DotNetExample.Domain.Commands;
using DotNetExample.Domain.Entities;
using DotNetExample.Domain.Queries;

using Microsoft.AspNetCore.Mvc;

using NSubstitute;

namespace DotNetExample.Tests.Api.StudentControllerTests;

[TestClass]
public partial class GetAllShould
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
        var students = new List<Student>
        {
            new Student { Id = "1", Name = "John Doe", Gpa = 3.5, Grade = "A" },
            new Student { Id = "2", Name = "Jane Smith", Gpa = 3.8, Grade = "A+" }
        };
        _getAllStudents.ExecuteAsync().Returns(students);

        // Act
        var result = await _sut.GetAll();

        // Assert
        Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        var okResult = (OkObjectResult)result;
        Assert.AreEqual(students, okResult.Value);
    }
}
