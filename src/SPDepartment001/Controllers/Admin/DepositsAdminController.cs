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
    public class DepositsAdminController : Controller
    {
        private IEmployeeRepository emplRepository;
        private IDepositRepository depositRepository;

        public DepositsAdminController(IEmployeeRepository emplRepo,
            IDepositRepository depositRepo)
        {
            emplRepository = emplRepo;
            depositRepository = depositRepo;
        }

        public ViewResult Index() => View(depositRepository.Deposits);

        public ViewResult Create()
        {
            PopulateViewBagEmployees();
            return View("Edit", new Deposit());
        }

        public ViewResult Edit(Guid depositId)
        {
            PopulateViewBagEmployees();

            return View(depositRepository.Deposits.FirstOrDefault(d => d.Id == depositId));
        }

        [HttpPost]
        public IActionResult Edit(Deposit deposit)
        {
            if (ModelState.IsValid)
            {
                depositRepository.SaveDeposit(deposit);
                //TempData["message"] = $"{employee.FirstName} {employee.LastName} has been saved";
                return RedirectToAction("Index");
            }
            else
            {
                // there is something wrong with the data values
                PopulateViewBagEmployees();
                return View(deposit);
            }
        }

        [HttpPost]
        public IActionResult Delete(Guid depositId)
        {
            Deposit deletedDeposit = depositRepository.DeleteDeposit(depositId);
            if (deletedDeposit != null)
            {
                //TempData["message"] = $"{deletedEmployee.FirstName} {deletedEmployee.LastName} was deleted";
            }
            return RedirectToAction("Index");
        }

        private void PopulateViewBagEmployees()
        {
            var employeesList = emplRepository.ActiveEmployees.Select(e => new { FullName = $"{e.FirstName} {e.LastName}", Id = e.EmployeeID });
            ViewBag.Employees = new SelectList(employeesList, "Id", "FullName");
        }
    }
}
