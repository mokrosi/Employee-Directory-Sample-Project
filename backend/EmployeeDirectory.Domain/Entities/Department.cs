using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeDirectory.Domain.Entities
{
    public class Department
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public int MaxHeadcount { get; set; }

        public ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}
