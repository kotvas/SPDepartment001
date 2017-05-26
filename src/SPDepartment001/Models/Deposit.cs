using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SPDepartment001.Models
{
    public class Deposit
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public decimal Amount { get; set; }

        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
    }
}
