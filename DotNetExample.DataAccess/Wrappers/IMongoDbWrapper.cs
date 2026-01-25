using DotNetExample.Domain.Entities;

namespace DotNetExample.DataAccess.Wrappers;

public interface IMongoDbWrapper
{
    Task<Student?> GetStudentByIdAsync(string id);
    Task<IEnumerable<Student>> GetAllStudentsAsync();
    Task InsertStudentAsync(Student student);
    Task UpdateStudentAsync(Student student);
    Task DeleteStudentAsync(string id);
}
