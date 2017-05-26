using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SPDepartment001.Models.Data
{
    public interface IExpenseRepository
    {
        IEnumerable<Expense> Expenses { get; }
        void SaveExpense(Expense expense);
        Expense DeleteExpense(Guid expenseId);
        bool GenerateExpensesByDepartmentEvent(Guid departmentEventId);
        void PaidExpense(Guid expenseId);
    }
}
