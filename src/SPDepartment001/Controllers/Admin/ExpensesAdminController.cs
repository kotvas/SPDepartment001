using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SPDepartment001.Models.Data;
using SPDepartment001.Models;
using Microsoft.AspNetCore.Authorization;
using SPDepartment001.Models.ViewModels;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace SPDepartment001.Controllers.Admin
{
    [Authorize(Roles = "Admins")]
    public class ExpensesAdminController : Controller
    {
        private IEmployeeRepository emplRepository;
        private IDepartmentEventRepository departmentEventRepository;
        private IExpenseRepository expenseRepository;
        private IEmployeeAccountRepository employeeAccountRepository;

        public ExpensesAdminController(IEmployeeRepository emplRepo,
            IDepartmentEventRepository departmentEventRepo,
            IExpenseRepository expenseRepo,
            IEmployeeAccountRepository emplAccountRepository)
        {
            emplRepository = emplRepo;
            departmentEventRepository = departmentEventRepo;
            expenseRepository = expenseRepo;
            employeeAccountRepository = emplAccountRepository;
        }

        public ViewResult Index(string filterStatus)
            => View(expenseRepository.Expenses
                .Where(e => filterStatus == null || (filterStatus == "notpaid" && !e.IsPaid)));

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

        public ViewResult AddExpenses(Guid departmentEventId)
        {
            DepartmentEvent departmentEvent = departmentEventRepository.DepartmentEvents.Where(dE => dE.Id == departmentEventId).FirstOrDefault();
            IEnumerable<Employee> addedEmployees = expenseRepository.Expenses.Where(e => e.DepartmentEventId == departmentEvent.Id).Select(e => e.Employee);
            IEnumerable<Employee> notAddedEmployees = emplRepository.Employees.Where(e => e.IsActive && !addedEmployees.Contains(e) && e.EmployeeID != departmentEvent.EmployeeId);

            return View(
                new AddExpensesViewModel()
                {
                    DepartmentEvent = departmentEvent,
                    AddedEmployees = addedEmployees,
                    NotAddedEmployees = notAddedEmployees
                });
        }

        [HttpPost]
        public IActionResult AddExpenses(AddExpensesModificationModel model)
        {
            bool isExpenseCreated = false;

            DepartmentEvent departmentEvent = departmentEventRepository.DepartmentEvents.Where(dE => dE.Id == new Guid(model.DepartmentEventId)).FirstOrDefault();

            foreach (string employeeIdStr in model.IdsToAdd ?? new string[] { })
            {
                int employeeId = Convert.ToInt32(employeeIdStr);
                Employee employee = emplRepository.Employees.Where(e => e.EmployeeID == employeeId).FirstOrDefault();

                if (departmentEvent != null && employee != null)
                {
                    expenseRepository.SaveExpense(
                        new Expense()
                        {
                            DepartmentEvent = departmentEvent,
                            DepartmentEventId = departmentEvent.Id,
                            Employee = employee,
                            EmployeeId = employeeId,
                            DateCreated = DateTime.Now,
                            Amount = departmentEvent.AmountOfEmployee
                        });

                    isExpenseCreated = true;
                }
            }

            if (!departmentEvent.AreExpensesGenerated && isExpenseCreated)
            {
                departmentEvent.AreExpensesGenerated = true;
                departmentEventRepository.SaveDepartmentEvent(departmentEvent);
            }

            return RedirectToAction("Index", "DepartmentEventsAdmin");
        }


    }
}
