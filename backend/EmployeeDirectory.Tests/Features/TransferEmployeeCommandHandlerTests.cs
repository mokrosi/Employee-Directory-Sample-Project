
using EmployeeDirectory.Application.Interfaces;
using EmployeeDirectory.Application.Features.Employees.Commands.TransferEmployee;
using EmployeeDirectory.Domain.Entities;
using EmployeeDirectory.Domain.Interfaces;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace EmployeeDirectory.Tests.Features;

public class TransferEmployeeCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldThrowException_WhenDepartmentIsAtCapacity()
    {
        // Arrange
        var mockEmployeeRepository = new Mock<IEmployeeRepository>();
        var mockDepartmentRepository = new Mock<IDepartmentRepository>();
        var mockCurrentUserService = new Mock<ICurrentUserService>();

        var handler = new TransferEmployeeCommandHandler(
            mockEmployeeRepository.Object,
            mockDepartmentRepository.Object,
            mockCurrentUserService.Object);

        var employeeId = Guid.NewGuid();
        var targetDepartmentId = Guid.NewGuid();

        var employee = new Employee { Id = employeeId, DepartmentId = Guid.NewGuid() };
        var targetDepartment = new Department { Id = targetDepartmentId, MaxHeadcount = 5 };

        mockEmployeeRepository.Setup(repo => repo.GetByIdAsync(employeeId)).ReturnsAsync(employee);
        mockDepartmentRepository.Setup(repo => repo.GetByIdAsync(targetDepartmentId)).ReturnsAsync(targetDepartment);
        mockEmployeeRepository.Setup(repo => repo.GetCountByDepartmentIdAsync(targetDepartmentId)).ReturnsAsync(5);

        var command = new TransferEmployeeCommand
        {
            EmployeeId = employeeId,
            NewDepartmentId = targetDepartmentId
        };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() =>
            handler.Handle(command, CancellationToken.None));

        Assert.Contains("maximum capacity", exception.Message);
        mockEmployeeRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Employee>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenDepartmentIsOverCapacity()
    {
        // Arrange
        var mockEmployeeRepository = new Mock<IEmployeeRepository>();
        var mockDepartmentRepository = new Mock<IDepartmentRepository>();
        var mockCurrentUserService = new Mock<ICurrentUserService>();

        var handler = new TransferEmployeeCommandHandler(
            mockEmployeeRepository.Object,
            mockDepartmentRepository.Object,
            mockCurrentUserService.Object);

        var employeeId = Guid.NewGuid();
        var targetDepartmentId = Guid.NewGuid();

        var employee = new Employee { Id = employeeId, DepartmentId = Guid.NewGuid() };
        var targetDepartment = new Department { Id = targetDepartmentId, MaxHeadcount = 5 };

        mockEmployeeRepository.Setup(repo => repo.GetByIdAsync(employeeId)).ReturnsAsync(employee);
        mockDepartmentRepository.Setup(repo => repo.GetByIdAsync(targetDepartmentId)).ReturnsAsync(targetDepartment);
        mockEmployeeRepository.Setup(repo => repo.GetCountByDepartmentIdAsync(targetDepartmentId)).ReturnsAsync(6);

        var command = new TransferEmployeeCommand
        {
            EmployeeId = employeeId,
            NewDepartmentId = targetDepartmentId
        };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() =>
            handler.Handle(command, CancellationToken.None));

        Assert.Contains("maximum capacity", exception.Message);
        mockEmployeeRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Employee>()), Times.Never);
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

        var handler = new TransferEmployeeCommandHandler(
            mockEmployeeRepository.Object,
            mockDepartmentRepository.Object,
            mockCurrentUserService.Object);

        var employeeId = Guid.NewGuid();
        var currentDeptId = Guid.NewGuid();
        var targetDepartmentId = Guid.NewGuid();

        var employee = new Employee { Id = employeeId, DepartmentId = currentDeptId };
        var targetDepartment = new Department { Id = targetDepartmentId, MaxHeadcount = 5 };

        mockEmployeeRepository.Setup(repo => repo.GetByIdAsync(employeeId)).ReturnsAsync(employee);
        mockDepartmentRepository.Setup(repo => repo.GetByIdAsync(targetDepartmentId)).ReturnsAsync(targetDepartment);
        mockEmployeeRepository.Setup(repo => repo.GetCountByDepartmentIdAsync(targetDepartmentId)).ReturnsAsync(3);

        var command = new TransferEmployeeCommand
        {
            EmployeeId = employeeId,
            NewDepartmentId = targetDepartmentId
        };

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result);
        mockEmployeeRepository.Verify(repo => repo.TransferAsync(
            employeeId,
            targetDepartmentId,
            It.Is<EmployeeDepartmentHistory>(h => h.EmployeeId == employeeId && h.DepartmentId == targetDepartmentId)), Times.Once);
    }
}