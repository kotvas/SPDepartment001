using Microsoft.EntityFrameworkCore;
using SPDepartment001.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SPDepartment001.Models.Data
{
    public class ExpenseRepository : IExpenseRepository
    {
        private ApplicationDbContext context;

        public ExpenseRepository(ApplicationDbContext ctx)
        {
            context = ctx;
        }

        public IEnumerable<Expense> Expenses
            => context.Expenses
                .Include(e => e.DepartmentEvent)
                    .ThenInclude(de => de.Employee)
                .Include(e => e.Employee)
                .OrderByDescending(e => e.DateCreated);

        public void SaveExpense(Expense expense)
        {
            if (expense.Id == new Guid())
            {
                context.Expenses.Add(expense);
            }
            else
            {
                //Deposit dbEntry = context.Deposits
                //    .FirstOrDefault(d => d.Id == deposit.Id);
                //if (dbEntry != null)
                //{
                //    dbEntry.Amount = deposit.Amount;
                //    dbEntry.EmployeeId = deposit.EmployeeId;
                //    dbEntry.Employee = deposit.Employee;
                //}
            }
            context.SaveChanges();
        }

        public Expense DeleteExpense(Guid expenseId)
        {
            Expense dbEntry = context.Expenses
                .FirstOrDefault(e => e.Id == expenseId);
            if (dbEntry != null)
            {
                context.Expenses.Remove(dbEntry);
                context.SaveChanges();
            }
            return dbEntry;
        }

        public bool GenerateExpensesByDepartmentEvent(Guid departmentEventId)
        {
            var result = false;

            var employeeEvent = context.DepartmentEvents.FirstOrDefault(e => e.Id == departmentEventId);
            var employees = context.Employees.Where(e => e.IsActive && e.EmployeeID != employeeEvent.EmployeeId).ToList();

            DateTime currentDate = DateTime.Now;

            using (var dbContextTransaction = context.Database.BeginTransaction())
            {
                try
                {
                    employees.ForEach(delegate (Employee employee)
                    {
                        var newExpense = new Expense()
                        {
                            DepartmentEventId = employeeEvent.Id,
                            EmployeeId = employee.EmployeeID,
                            Amount = employeeEvent.AmountOfEmployee,
                            DateCreated = currentDate
                        };
                        context.Expenses.Add(newExpense);
                        context.SaveChanges();
                    });

                    employeeEvent.AreExpensesGenerated = true;
                    context.SaveChanges();

                    dbContextTransaction.Commit();

                    result = true;
                }
                catch
                {
                    dbContextTransaction.Rollback();
                }
            }

            return result;
        }

        public void PaidExpense(Guid expenseId)
        {
            Expense expense = context.Expenses
                .Where(exp => exp.Id == expenseId)
                .FirstOrDefault();

            Employee employee = context.Employees
                    .Where(empl => empl.EmployeeID == expense.EmployeeId)
                    .FirstOrDefault();

            EmployeeAccount employeeAccount = context.EmployeesAccounts
                .Where(eA => eA.EmployeeId == expense.EmployeeId)
                .FirstOrDefault();

            using (var dbContextTransaction = context.Database.BeginTransaction())
            {
                try
                {
                    if (employeeAccount == null)
                    {
                        var newEmployeeAccount = new EmployeeAccount()
                        {
                            EmployeeId = employee.EmployeeID,
                            Employee = employee,
                            Amount = expense.Amount,
                            DateOfLastUpdate = DateTime.Now
                        };

                        context.EmployeesAccounts.Add(newEmployeeAccount);
                        context.SaveChanges();
                    }
                    else
                    {
                        employeeAccount.Amount = employeeAccount.Amount - expense.Amount;
                        employeeAccount.DateOfLastUpdate = DateTime.Now;

                        context.SaveChanges();
                    }

                    expense.IsPaid = true;
                    expense.DateOfPayment = DateTime.Now;
                    context.SaveChanges();

                    dbContextTransaction.Commit();
                }
                catch
                {
                    dbContextTransaction.Rollback();
                }
            }
        }

    }
}
