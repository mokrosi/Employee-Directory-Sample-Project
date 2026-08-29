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

    public async Task<bool> IsEmailUniqueAsync(string email, Guid? excludeEmployeeId = null)
    {
        var normalizedEmail = email.Trim().ToLower();
        return !await _context.Employees.AnyAsync(e =>
            e.Email.ToLower() == normalizedEmail &&
            (!excludeEmployeeId.HasValue || e.Id != excludeEmployeeId.Value));
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
            .OrderByDescending(e => e.CreatedAt)
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

    public async Task AddHistoryAsync(EmployeeDepartmentHistory history)
    {
        await _context.EmployeeDepartmentHistories.AddAsync(history);
    }

    public async Task<IEnumerable<EmployeeDepartmentHistory>> GetAllHistoryAsync()
    {
        return await _context.EmployeeDepartmentHistories
            .Include(h => h.Employee)
            .Include(h => h.Department)
            .Include(h => h.TransferredByUser)
            .OrderByDescending(h => h.TransferredAt)
            .ToListAsync();
    }

    public async Task TransferAsync(Guid employeeId, Guid newDepartmentId, EmployeeDepartmentHistory history)
    {
        // Clear the change tracker to discard all tracked entities from prior Include() queries.
        // This prevents EF Core from generating phantom UPDATEs on related Department/History entities.
        _context.ChangeTracker.Clear();

        // Direct SQL UPDATE — bypasses the change tracker entirely, no concurrency conflict possible
        await _context.Employees
            .Where(e => e.Id == employeeId)
            .ExecuteUpdateAsync(s => s.SetProperty(e => e.DepartmentId, newDepartmentId));

        // Insert the history record
        await _context.EmployeeDepartmentHistories.AddAsync(history);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Employee employee)
    {
        _context.Employees.Remove(employee);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Employee employee)
    {
        await _context.SaveChangesAsync();
    }
}