using Xunit;
using BLL.Interfaces;
using BLL.Models;
using Moq;

public class StudentServiceTests
{
    [Fact]
    public void GetAllStudents_ReturnsAllStudents()
    {
        // Arrange
        var mockService = new Mock<IStudentRepos>();
        mockService.Setup(service => service.GetAllStudents()).Returns(new List<Student>
        {
            new Student { Id = 1, Name = "John Doe" },
            new Student { Id = 2, Name = "Jane Doe" }
        });

        // Act
        var students = mockService.Object.GetAllStudents();

        // Assert
        Assert.Equal(2, students.Count());
    }
}
