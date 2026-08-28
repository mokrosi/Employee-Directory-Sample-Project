using EmployeeDirectory.Domain.Entities;
using EmployeeDirectory.Domain.Interfaces;
using EmployeeDirectory.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace EmployeeDirectory.Infrastructure.Repositories;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly AppDbContext _context;

    public EmployeeRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Employee> AddAsync(Employee employee)
    {
        await _context.Employees.AddAsync(employee);
        await _context.SaveChangesAsync();
        return employee;
    }

    public async Task<bool> IsEmployeeCodeUniqueAsync(string employeeCode)
    {
        return !await _context.Employees.AnyAsync(e => e.EmployeeCode == employeeCode);
    }

    public async Task<int> GetCountByDepartmentIdAsync(Guid departmentId)
    {
        return await _context.Employees.CountAsync(e => e.DepartmentId == departmentId);
    }

    public async Task<(IEnumerable<Employee> Employees, int TotalCount)> SearchAsync(string searchTerm, int pageNumber, int pageSize)
    {
        var query = _context.Employees.Include(e => e.Department).AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            searchTerm = searchTerm.ToLower();
            query = query.Where(e =>
                e.FullName.ToLower().Contains(searchTerm) ||
                e.Email.ToLower().Contains(searchTerm) ||
                (e.Department != null && e.Department.Name.ToLower().Contains(searchTerm)));
        }

        var totalCount = await query.CountAsync();

        var employees = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (employees, totalCount);
    }

    public async Task<Employee?> GetByIdAsync(Guid id)
    {
        return await _context.Employees
            .Include(e => e.Department)
            .Include(e => e.DepartmentHistories)
                .ThenInclude(h => h.Department)
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task DeleteAsync(Employee employee)
    {
        _context.Employees.Remove(employee);
        await _context.SaveChangesAsync();
    }


    public async Task UpdateAsync(Employee employee)
    {
        _context.Employees.Update(employee);
        await _context.SaveChangesAsync();
    }
}