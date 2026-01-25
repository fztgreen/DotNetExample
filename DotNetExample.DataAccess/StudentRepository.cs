using DotNetExample.DataAccess.Wrappers;
using DotNetExample.Domain.DataAccess;
using DotNetExample.Domain.Entities;

namespace DotNetExample.DataAccess;

public class StudentRepository : IStudentRepository
{
    private readonly IMongoDbWrapper _mongoDbWrapper;

    public StudentRepository(IMongoDbWrapper mongoDbWrapper)
    {
        _mongoDbWrapper = mongoDbWrapper;
    }

    public Task<Student?> GetByIdAsync(string id)
    {
        return _mongoDbWrapper.GetStudentByIdAsync(id);
    }

    public Task<IEnumerable<Student>> GetAllAsync()
    {
        return _mongoDbWrapper.GetAllStudentsAsync();
    }

    public Task CreateAsync(Student student)
    {
        return _mongoDbWrapper.InsertStudentAsync(student);
    }

    public Task UpdateAsync(Student student)
    {
        return _mongoDbWrapper.UpdateStudentAsync(student);
    }

    public Task DeleteAsync(string id)
    {
        return _mongoDbWrapper.DeleteStudentAsync(id);
    }
}
