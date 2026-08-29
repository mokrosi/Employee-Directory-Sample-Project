using System;
using System.Collections.Generic;
using System.Text;
using EmployeeDirectory.Domain.Entities;
using EmployeeDirectory.Domain.Interfaces;
using EmployeeDirectory.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EmployeeDirectory.Infrastructure.Repositories;

public class DepartmentRepository : IDepartmentRepository
{
    private readonly AppDbContext _context;

    public DepartmentRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Department> AddAsync(Department department)
    {
        await _context.Departments.AddAsync(department);
        await _context.SaveChangesAsync();
        return department;
    }


    public async Task<bool> IsNameUniqueAsync(string name, Guid? excludeDepartmentId = null)
    {
        var normalizedName = name.Trim().ToLower();
        return !await _context.Departments.AnyAsync(d =>
            d.Name.ToLower() == normalizedName &&
            (!excludeDepartmentId.HasValue || d.Id != excludeDepartmentId.Value));
    }

    public async Task<Department?> GetByIdAsync(Guid id)
    {
        return await _context.Departments.Include(d => d.Employees).FirstOrDefaultAsync(d => d.Id == id);
    }

    public async Task<IEnumerable<Department>> GetAllAsync()
    {
        return await _context.Departments.Include(d => d.Employees).ToListAsync();
    }

    public async Task UpdateAsync(Department department)
    {
        await _context.SaveChangesAsync();
    }
}