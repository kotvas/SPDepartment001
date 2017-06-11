using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SPDepartment001.Models.ViewModels
{
    public class AddExpensesViewModel
    {
        public DepartmentEvent DepartmentEvent { get; set; }
        public IEnumerable<Employee> AddedEmployees { get; set; }
        public IEnumerable<Employee> NotAddedEmployees { get; set; }
    }

    public class AddExpensesModificationModel
    {
        public string DepartmentEventId { get; set; }
        public string[] IdsToAdd { get; set; }
    }
}
