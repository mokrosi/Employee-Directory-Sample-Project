using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeDirectory.Domain.Entities
{
    internal class Employee
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string EmployeeCode { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        // Foreign Keys
        public Guid DepartmentId { get; set; }
        public Department? Department { get; set; }

        public Guid CreatedByUserId { get; set; }
        public User? CreatedByUser { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<EmployeeDepartmentHistory> DepartmentHistories { get; set; } = new List<EmployeeDepartmentHistory>();
    }
}
}
