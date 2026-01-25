using DotNetExample.Domain.DataAccess;
using DotNetExample.Domain.Entities;

namespace DotNetExample.Domain.Queries;

public class GetStudent : IGetStudent
{
    private readonly IStudentRepository _studentRepository;

    public GetStudent(IStudentRepository studentRepository)
    {
        _studentRepository = studentRepository;
    }

    public Task<Student?> ExecuteAsync(string id)
    {
        return _studentRepository.GetByIdAsync(id);
    }
}
