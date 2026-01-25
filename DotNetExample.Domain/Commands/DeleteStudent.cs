using DotNetExample.Domain.DataAccess;

namespace DotNetExample.Domain.Commands;

public class DeleteStudent : IDeleteStudent
{
    private readonly IStudentRepository _studentRepository;

    public DeleteStudent(IStudentRepository studentRepository)
    {
        _studentRepository = studentRepository;
    }

    public Task ExecuteAsync(string id)
    {
        return _studentRepository.DeleteAsync(id);
    }
}
