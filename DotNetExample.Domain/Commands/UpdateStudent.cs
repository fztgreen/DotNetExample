using DotNetExample.Domain.DataAccess;
using DotNetExample.Domain.Entities;

namespace DotNetExample.Domain.Commands;

public class UpdateStudent : IUpdateStudent
{
    private readonly IStudentRepository _studentRepository;

    public UpdateStudent(IStudentRepository studentRepository)
    {
        _studentRepository = studentRepository;
    }

    public Task ExecuteAsync(Student student)
    {
        return _studentRepository.UpdateAsync(student);
    }
}
