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


    public async Task<bool> IsNameUniqueAsync(string name)
    {
        return !await _context.Departments.AnyAsync(d => d.Name == name);
    }

    public async Task<Department?> GetByIdAsync(Guid id)
    {
        return await _context.Departments.FindAsync(id);
    }

    public async Task<IEnumerable<Department>> GetAllAsync()
    {
        return await _context.Departments.ToListAsync();
    }
}