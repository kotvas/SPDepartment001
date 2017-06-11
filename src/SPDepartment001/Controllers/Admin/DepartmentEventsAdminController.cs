using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SPDepartment001.Models.Data;
using SPDepartment001.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace SPDepartment001.Controllers.Admin
{
    [Authorize(Roles = "Admins")]
    public class DepartmentEventsAdminController : Controller
    {
        private IEmployeeRepository emplRepository;
        private IDepartmentEventRepository departmentEventRepository;
        private IExpenseRepository expenseRepository;

        public DepartmentEventsAdminController(IEmployeeRepository emplRepo,
            IDepartmentEventRepository departmentEventRepo,
            IExpenseRepository expenseRepo)
        {
            emplRepository = emplRepo;
            departmentEventRepository = departmentEventRepo;
            expenseRepository = expenseRepo;
        }

        public ViewResult Index() => View(departmentEventRepository.DepartmentEvents);

        public ViewResult Create()
        {
            PopulateViewBagEmployees();
            return View("Edit", new DepartmentEvent());
        }

        public ViewResult Edit(Guid departmentEventId)
        {
            PopulateViewBagEmployees();

            return View(departmentEventRepository.DepartmentEvents.FirstOrDefault(de => de.Id == departmentEventId));
        }

        [HttpPost]
        public IActionResult Edit(DepartmentEvent departmentEvent)
        {
            if (ModelState.IsValid)
            {
                departmentEventRepository.SaveDepartmentEvent(departmentEvent);
                //TempData["message"] = $"{employee.FirstName} {employee.LastName} has been saved";
                return RedirectToAction("Index");
            }
            else
            {
                // there is something wrong with the data values
                PopulateViewBagEmployees();
                return View(departmentEvent);
            }
        }

        [HttpPost]
        public IActionResult Delete(Guid departmentEventId)
        {
            DepartmentEvent departmentEvent = departmentEventRepository.DepartmentEvents.FirstOrDefault(dE => dE.Id == departmentEventId);
            IEnumerable<Expense> existingExpenses = expenseRepository.Expenses.Where(e => e.DepartmentEventId == departmentEventId);
            if (existingExpenses.Count() == 0)
            {
                DepartmentEvent deleteddepartmentEvent = departmentEventRepository.DeleteDepartmentEvent(departmentEventId);
                if (deleteddepartmentEvent != null)
                {
                    TempData["message"] = $"Department Event for {deleteddepartmentEvent.Employee.FirstName} {deleteddepartmentEvent.Employee.LastName} was deleted";
                }
            }
            else
            {
                TempData["message"] = $"Error: Department Event for {departmentEvent.Employee.FirstName} {departmentEvent.Employee.LastName} can't be deleted. There are existings expenses.";
            }
            return RedirectToAction("Index");
        }

        private void PopulateViewBagEmployees()
        {
            var employeesList = emplRepository.Employees.Select(e => new { FullName = $"{e.FirstName} {e.LastName}", Id = e.EmployeeID });
            ViewBag.Employees = new SelectList(employeesList, "Id", "FullName");
        }

        [HttpPost]
        public IActionResult GenerateExpenses(Guid departmentEventId)
        {
            if (expenseRepository.GenerateExpensesByDepartmentEvent(departmentEventId))
            {
                TempData["message"] = $"Expenses were generated successfully";
            }
            else
            {
                TempData["message"] = $"Error: Expenses weren't generated. Please try again";
            }

            return RedirectToAction("Index");
        }
    }
}
