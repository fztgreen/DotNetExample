namespace DotNetExample.Domain.Entities;

public class Student
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public double Gpa { get; set; }
    public string Grade { get; set; } = string.Empty;
}
