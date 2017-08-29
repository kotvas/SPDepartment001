using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SPDepartment001.Models.ViewModels
{
    public class ExpensesViewModel
    {
        public IEnumerable<Expense> Expenses { get; set; }
        public decimal TotalExpensesAmount { get; set; }
    }
}
