using DotNetExample.Domain.Entities;

namespace DotNetExample.Domain.Commands;

public interface ICreateStudent
{
    Task ExecuteAsync(Student student);
}
