using DotNetExample.Domain.DataAccess;
using DotNetExample.Domain.Entities;

namespace DotNetExample.Domain.Queries;

public class GetAllStudents : IGetAllStudents
{
    private readonly IStudentRepository _studentRepository;

    public GetAllStudents(IStudentRepository studentRepository)
    {
        _studentRepository = studentRepository;
    }

    public Task<IEnumerable<Student>> ExecuteAsync()
    {
        return _studentRepository.GetAllAsync();
    }
}
