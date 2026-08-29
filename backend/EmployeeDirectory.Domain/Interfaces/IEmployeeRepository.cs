using EmployeeDirectory.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeDirectory.Domain.Interfaces
{
    public interface IEmployeeRepository
    {
        Task<Employee> AddAsync(Employee employee);
        Task<bool> IsEmployeeCodeUniqueAsync(string employeeCode);
        Task<bool> IsEmailUniqueAsync(string email, Guid? excludeEmployeeId = null);
        Task<int> GetCountByDepartmentIdAsync(Guid departmentId);
        Task<(IEnumerable<Employee> Employees, int TotalCount)> SearchAsync(string searchTerm, int pageNumber, int pageSize);
        Task<Employee?> GetByIdAsync(Guid id);

        Task AddHistoryAsync(EmployeeDepartmentHistory history);
        Task<IEnumerable<EmployeeDepartmentHistory>> GetAllHistoryAsync();
        Task TransferAsync(Guid employeeId, Guid newDepartmentId, EmployeeDepartmentHistory history);
        Task UpdateAsync(Employee employee);
        Task DeleteAsync(Employee employee);

        
    }
}
