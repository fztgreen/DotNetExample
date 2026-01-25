using DotNetExample.Domain.Entities;

namespace DotNetExample.Domain.Queries;

public interface IGetStudent
{
    Task<Student?> ExecuteAsync(string id);
}
