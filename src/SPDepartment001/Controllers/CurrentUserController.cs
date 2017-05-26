using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SPDepartment001.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using SPDepartment001.Models.Identity;
using SPDepartment001.Models.Data;
using SPDepartment001.Models;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace SPDepartment001.Controllers
{
    [Authorize]
    public class CurrentUserController : Controller
    {
        private UserManager<AppUser> userManager;
        private IEmployeeRepository employeeRepository;
        private IEmployeeAccountRepository employeeAccountRepository;
        private IDepositRepository depositRepository;
        private IExpenseRepository expenseRepository;

        public CurrentUserController(UserManager<AppUser> usrMgr,
            IEmployeeRepository employeeRepo,
            IEmployeeAccountRepository employeeAccountRepo,
            IDepositRepository depositRepo,
            IExpenseRepository expenseRepo)
        {
            userManager = usrMgr;
            employeeRepository = employeeRepo;
            employeeAccountRepository = employeeAccountRepo;
            depositRepository = depositRepo;
            expenseRepository = expenseRepo;
        }

        public async Task<IActionResult> Info()
        {
            CurrentUserInfoViewModel currentUserInfo = new CurrentUserInfoViewModel();

            AppUser user = await userManager.FindByNameAsync(HttpContext.User.Identity.Name);
            if (user != null)
            {
                currentUserInfo.AppUser = user;

                Employee employee = employeeRepository.Employees.Where(e => e.AssociatedUserId.ToString() == user.Id).FirstOrDefault();

                if (employee != null)
                {
                    currentUserInfo.Employee = employee;
                    currentUserInfo.EmployeeAccount = employeeAccountRepository.EmployeesAccounts.Where(ea => ea.EmployeeId == employee.EmployeeID).FirstOrDefault();
                    currentUserInfo.Deposits = depositRepository.Deposits.Where(d => d.EmployeeId == employee.EmployeeID);
                    currentUserInfo.Expenses = expenseRepository.Expenses.Where(e => e.EmployeeId == employee.EmployeeID && !e.IsPaid);
                }
            }

            return View(currentUserInfo);
        }
    }
}
