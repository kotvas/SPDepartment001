using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SPDepartment001.Models.Data;
using SPDepartment001.Models.ViewModels;
using SPDepartment001.Models;
using Microsoft.AspNetCore.Identity;
using SPDepartment001.Models.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace SPDepartment001.Controllers.Admin
{
    [Authorize(Roles = "Admins")]
    public class EmployeesAdminController : Controller
    {
        private IEmployeeRepository repository;
        private UserManager<AppUser> userManager;

        public EmployeesAdminController(IEmployeeRepository repo,
            UserManager<AppUser> usrMgr)
        {
            repository = repo;
            userManager = usrMgr;
        }

        public ViewResult Index() => View(repository.Employees.OrderBy(e => e.LastName));

        public ViewResult Edit(int employeeId)
        {
            PopulateViewBagUsers();

            return View(repository.Employees.FirstOrDefault(e => e.EmployeeID == employeeId));
        }

        [HttpPost]
        public IActionResult Edit(Employee employee)
        {
            if (ModelState.IsValid)
            {
                repository.SaveEmployee(employee);
                //TempData["message"] = $"{employee.FirstName} {employee.LastName} has been saved";
                return RedirectToAction("Index");
            }
            else
            {
                // there is something wrong with the data values
                PopulateViewBagUsers();
                return View(employee);
            }
        }

        public ViewResult Create()
        {
            PopulateViewBagUsers();
            return View("Edit", new Employee());
        }

        [HttpPost]
        public IActionResult Delete(int employeeId)
        {
            Employee deletedEmployee = repository.DeleteEmployee(employeeId);
            if (deletedEmployee != null)
            {
                //TempData["message"] = $"{deletedEmployee.FirstName} {deletedEmployee.LastName} was deleted";
            }
            return RedirectToAction("Index");
        }

        private void PopulateViewBagUsers()
        {
            ViewBag.Users = new SelectList(userManager.Users, "Id", "UserName");
        }
    }
}
