using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SPDepartment001.Models
{
    public class Expense
    {
        public Guid Id { get; set; }

        public Guid DepartmentEventId { get; set; }
        public DepartmentEvent DepartmentEvent { get; set; }

        public int? EmployeeId { get; set; }
        public Employee Employee { get; set; }

        public decimal Amount { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public DateTime? DateOfPayment { get; set; }
        public bool IsPaid { get; set; } = false;

    }
}
