using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeDirectory.Domain.Entities
{
    internal class EmployeeDepartmentHistory
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid EmployeeId { get; set; }
        public Employee? Employee { get; set; }

        public Guid DepartmentId { get; set; }
        public Department? Department { get; set; }

        public Guid TransferredByUserId { get; set; }
        public User? TransferredByUser { get; set; }

        public DateTime TransferredAt { get; set; } = DateTime.UtcNow;
    }
}
