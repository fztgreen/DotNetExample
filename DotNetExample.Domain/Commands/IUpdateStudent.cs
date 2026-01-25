using DotNetExample.Domain.Entities;

namespace DotNetExample.Domain.Commands;

public interface IUpdateStudent
{
    Task ExecuteAsync(Student student);
}
