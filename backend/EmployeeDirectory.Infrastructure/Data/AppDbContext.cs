using EmployeeDirectory.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EmployeeDirectory.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Department> Departments => Set<Department>();
    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<EmployeeDepartmentHistory> EmployeeDepartmentHistories => Set<EmployeeDepartmentHistory>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Department Configuration
        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(d => d.Id);
            entity.Property(d => d.Name).IsRequired().HasMaxLength(100);
            entity.Property(d => d.MaxHeadcount).IsRequired();
        });

        // User Configuration
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("Users");
            entity.HasKey(u => u.Id);
        });

        // Employee Configuration
        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.EmployeeCode).IsUnique();
            entity.HasIndex(e => e.Email).IsUnique();
            entity.Property(e => e.FullName).IsRequired().HasMaxLength(150);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(200);

            entity.HasOne(e => e.Department)
                  .WithMany(d => d.Employees)
                  .HasForeignKey(e => e.DepartmentId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.CreatedByUser)
                  .WithMany()
                  .HasForeignKey(e => e.CreatedByUserId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.Property(e => e.CreatedByUserId).IsRequired();
            entity.Property(e => e.CreatedAt).IsRequired();
        });

        // Assignment History Configuration
        modelBuilder.Entity<EmployeeDepartmentHistory>(entity =>
        {
            entity.HasKey(h => h.Id);

            entity.HasOne(h => h.Employee)
                  .WithMany(e => e.DepartmentHistories)
                  .HasForeignKey(h => h.EmployeeId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(h => h.Department)
                  .WithMany()
                  .HasForeignKey(h => h.DepartmentId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(h => h.TransferredByUser)
                  .WithMany()
                  .HasForeignKey(h => h.TransferredByUserId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.Property(h => h.TransferredByUserId).IsRequired();
            entity.Property(h => h.TransferredAt).IsRequired();
        });
    }
}