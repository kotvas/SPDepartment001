using SPDepartment001.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SPDepartment001.Models.ViewModels
{
    public class CurrentUserInfoViewModel
    {
        public AppUser AppUser { get; set; }
        public Employee Employee { get; set; }
        public EmployeeAccount EmployeeAccount { get; set; }
        public IEnumerable<Deposit> Deposits { get; set; }
        public ExpensesViewModel ExpensesInfo { get; set; }
    }
}
