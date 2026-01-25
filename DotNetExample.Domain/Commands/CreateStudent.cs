using DotNetExample.Domain.DataAccess;
using DotNetExample.Domain.Entities;

namespace DotNetExample.Domain.Commands;

public class CreateStudent : ICreateStudent
{
    private readonly IStudentRepository _studentRepository;

    public CreateStudent(IStudentRepository studentRepository)
    {
        _studentRepository = studentRepository;
    }

    public Task ExecuteAsync(Student student)
    {
        return _studentRepository.CreateAsync(student);
    }
}
