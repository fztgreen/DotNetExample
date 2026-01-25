using DotNetExample.Domain.Entities;

namespace DotNetExample.Domain.DataAccess;

public interface IStudentRepository
{
    Task<Student?> GetByIdAsync(string id);
    Task<IEnumerable<Student>> GetAllAsync();
    Task CreateAsync(Student student);
    Task UpdateAsync(Student student);
    Task DeleteAsync(string id);
}
