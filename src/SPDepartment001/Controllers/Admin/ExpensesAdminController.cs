using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SPDepartment001.Models.Data;
using SPDepartment001.Models;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace SPDepartment001.Controllers.Admin
{
    [Authorize(Roles = "Admins")]
    public class ExpensesAdminController : Controller
    {
        //private IEmployeeRepository emplRepository;
        //private IDepartmentEventRepository departmentEventRepository;
        private IExpenseRepository expenseRepository;
        private IEmployeeAccountRepository employeeAccountRepository;

        public ExpensesAdminController(//IEmployeeRepository emplRepo,
            //IDepartmentEventRepository departmentEventRepo,
            IExpenseRepository expenseRepo,
            IEmployeeAccountRepository emplAccountRepository)
        {
            //emplRepository = emplRepo;
            //departmentEventRepository = departmentEventRepo;
            expenseRepository = expenseRepo;
            employeeAccountRepository = emplAccountRepository;
        }

        public ViewResult Index() => View(expenseRepository.Expenses);

        public ViewResult Create()
        {
            //PopulateViewBagEmployees();
            return View("Edit", new Expense());
        }

        public ViewResult Edit(Guid expenseId)
        {
            //PopulateViewBagEmployees();

            return View(expenseRepository.Expenses.FirstOrDefault(e => e.Id == expenseId));
        }

        [HttpPost]
        public IActionResult Edit(Expense expense)
        {
            if (ModelState.IsValid)
            {
                expenseRepository.SaveExpense(expense);
                //TempData["message"] = $"{employee.FirstName} {employee.LastName} has been saved";
                return RedirectToAction("Index");
            }
            else
            {
                // there is something wrong with the data values
                //PopulateViewBagEmployees();
                return View(expense);
            }
        }

        [HttpPost]
        public IActionResult Delete(Guid expenseId)
        {
            Expense deletedExpense = expenseRepository.DeleteExpense(expenseId);
            if (deletedExpense != null)
            {
                //TempData["message"] = $"{deletedEmployee.FirstName} {deletedEmployee.LastName} was deleted";
            }
            return RedirectToAction("Index");
        }

        //private void PopulateViewBagEmployees()
        //{
        //    ViewBag.Employees = new SelectList(emplRepository.Employees, "EmployeeID", "FirstName");
        //}

        [HttpPost]
        public IActionResult Pay(Guid expenseId)
        {
            Expense expense = expenseRepository.Expenses.Where(e => e.Id == expenseId).FirstOrDefault();
            if (expense?.Id != new Guid())
            {
                EmployeeAccount employeeAccount = employeeAccountRepository.EmployeesAccounts.Where(ea => ea.EmployeeId == expense.Employee.EmployeeID).FirstOrDefault();
                decimal employeeAccountAmount = employeeAccount?.Amount ?? 0;

                if (expense.Amount <= employeeAccountAmount)
                {
                    expenseRepository.PaidExpense(expenseId);
                    TempData["message"] = $"{expense.Employee.FirstName} {expense.Employee.LastName} event was paid successfully";
                }
                else
                {
                    TempData["message"] = $"Error: User doesn't have enough money";
                }
            }
            else
            {
                TempData["message"] = $"Error: Expense for {expense.Employee.FirstName} {expense.Employee.LastName} wasn't found";
            }

            return RedirectToAction("Index");
        }
        
    }
}
