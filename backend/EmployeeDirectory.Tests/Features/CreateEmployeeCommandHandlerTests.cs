using EmployeeDirectory.Application.Interfaces;
using EmployeeDirectory.Application.Features.Employees.Commands.CreateEmployee;
using EmployeeDirectory.Domain.Entities;
using EmployeeDirectory.Domain.Interfaces;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace EmployeeDirectory.Tests.Features;

public class CreateEmployeeCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldThrowException_WhenDepartmentIsAtCapacity()
    {
        // Arrange
        var mockEmployeeRepository = new Mock<IEmployeeRepository>();
        var mockDepartmentRepository = new Mock<IDepartmentRepository>();
        var mockCurrentUserService = new Mock<ICurrentUserService>();

        var handler = new CreateEmployeeCommandHandler(
            mockEmployeeRepository.Object,
            mockDepartmentRepository.Object,
            mockCurrentUserService.Object);

        var deptId = Guid.NewGuid();
        var department = new Department { Id = deptId, MaxHeadcount = 3 };

        mockEmployeeRepository.Setup(r => r.IsEmployeeCodeUniqueAsync("EMP001")).ReturnsAsync(true);
        mockEmployeeRepository.Setup(r => r.IsEmailUniqueAsync("mohammed@example.com", null)).ReturnsAsync(true);
        mockDepartmentRepository.Setup(r => r.GetByIdAsync(deptId)).ReturnsAsync(department);
        mockEmployeeRepository.Setup(r => r.GetCountByDepartmentIdAsync(deptId)).ReturnsAsync(3);

        var command = new CreateEmployeeCommand
        {
            EmployeeCode = "EMP001",
            FullName = "mohammed",
            Email = "mohammed@example.com",
            DepartmentId = deptId
        };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() =>
            handler.Handle(command, CancellationToken.None));

        Assert.Contains("maximum capacity", exception.Message);
        mockEmployeeRepository.Verify(r => r.AddAsync(It.IsAny<Employee>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenEmailIsAlreadyInUse()
    {
        // Arrange
        var mockEmployeeRepository = new Mock<IEmployeeRepository>();
        var mockDepartmentRepository = new Mock<IDepartmentRepository>();
        var mockCurrentUserService = new Mock<ICurrentUserService>();

        var handler = new CreateEmployeeCommandHandler(
            mockEmployeeRepository.Object,
            mockDepartmentRepository.Object,
            mockCurrentUserService.Object);

        var deptId = Guid.NewGuid();
        var department = new Department { Id = deptId, MaxHeadcount = 3 };

        mockEmployeeRepository.Setup(r => r.IsEmployeeCodeUniqueAsync("EMP001")).ReturnsAsync(true);
        mockEmployeeRepository.Setup(r => r.IsEmailUniqueAsync("mohammed@example.com", null)).ReturnsAsync(false);
        mockDepartmentRepository.Setup(r => r.GetByIdAsync(deptId)).ReturnsAsync(department);

        var command = new CreateEmployeeCommand
        {
            EmployeeCode = "EMP001",
            FullName = "mohammed",
            Email = "mohammed@example.com",
            DepartmentId = deptId
        };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() =>
            handler.Handle(command, CancellationToken.None));

        Assert.Contains("email is already in use", exception.Message, StringComparison.OrdinalIgnoreCase);
        mockEmployeeRepository.Verify(r => r.AddAsync(It.IsAny<Employee>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenDepartmentIsOverCapacity()
    {
        // Arrange
        var mockEmployeeRepository = new Mock<IEmployeeRepository>();
        var mockDepartmentRepository = new Mock<IDepartmentRepository>();
        var mockCurrentUserService = new Mock<ICurrentUserService>();

        var handler = new CreateEmployeeCommandHandler(
            mockEmployeeRepository.Object,
            mockDepartmentRepository.Object,
            mockCurrentUserService.Object);

        var deptId = Guid.NewGuid();
        var department = new Department { Id = deptId, MaxHeadcount = 3 };

        mockEmployeeRepository.Setup(r => r.IsEmployeeCodeUniqueAsync("EMP001")).ReturnsAsync(true);
        mockEmployeeRepository.Setup(r => r.IsEmailUniqueAsync("mohammed@example.com", null)).ReturnsAsync(true);
        mockDepartmentRepository.Setup(r => r.GetByIdAsync(deptId)).ReturnsAsync(department);
        mockEmployeeRepository.Setup(r => r.GetCountByDepartmentIdAsync(deptId)).ReturnsAsync(4);

        var command = new CreateEmployeeCommand
        {
            EmployeeCode = "EMP001",
            FullName = "mohammed",
            Email = "mohammed@example.com",
            DepartmentId = deptId
        };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() =>
            handler.Handle(command, CancellationToken.None));

        Assert.Contains("maximum capacity", exception.Message);
        mockEmployeeRepository.Verify(r => r.AddAsync(It.IsAny<Employee>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldSucceed_WhenDepartmentHasAvailableCapacity()
    {
        // Arrange
        var mockEmployeeRepository = new Mock<IEmployeeRepository>();
        var mockDepartmentRepository = new Mock<IDepartmentRepository>();
        var mockCurrentUserService = new Mock<ICurrentUserService>();
        var currentUserId = Guid.NewGuid();
        mockCurrentUserService.Setup(s => s.UserId).Returns(currentUserId);

        var handler = new CreateEmployeeCommandHandler(
            mockEmployeeRepository.Object,
            mockDepartmentRepository.Object,
            mockCurrentUserService.Object);

        var deptId = Guid.NewGuid();
        var department = new Department { Id = deptId, MaxHeadcount = 3 };

        mockEmployeeRepository.Setup(r => r.IsEmployeeCodeUniqueAsync("EMP001")).ReturnsAsync(true);
        mockEmployeeRepository.Setup(r => r.IsEmailUniqueAsync("mohammed@example.com", null)).ReturnsAsync(true);
        mockDepartmentRepository.Setup(r => r.GetByIdAsync(deptId)).ReturnsAsync(department);
        mockEmployeeRepository.Setup(r => r.GetCountByDepartmentIdAsync(deptId)).ReturnsAsync(1);

        var command = new CreateEmployeeCommand
        {
            EmployeeCode = "EMP001",
            FullName = "mohammed",
            Email = "mohammed@example.com",
            DepartmentId = deptId
        };

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotEqual(Guid.Empty, result);
        mockEmployeeRepository.Verify(r => r.AddAsync(It.Is<Employee>(e =>
            e.EmployeeCode == "EMP001" &&
            e.DepartmentId == deptId &&
            e.CreatedByUserId == currentUserId &&
            e.DepartmentHistories.Count == 1)), Times.Once);
    }
}
