using DotNetExample.Domain.Entities;

namespace DotNetExample.Domain.Queries;

public interface IGetAllStudents
{
    Task<IEnumerable<Student>> ExecuteAsync();
}
