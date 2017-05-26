using Microsoft.AspNetCore.Mvc;
using SPDepartment001.Models.Data;
using System.Linq;
using SPDepartment001.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace SPDepartment001.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        private IEmployeeRepository repository;

        public int PageSize = 10;

        public EmployeeController(IEmployeeRepository repo)
        {
            repository = repo;
        }

        public ViewResult List(int page = 1)
            => View(new EmployeesListViewModel
            {
                Employees = repository.Employees
                    .OrderBy(e => e.LastName)
                    .Where(e => e.IsActive)
                    .Skip((page - 1) * PageSize)
                    .Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = repository.Employees.Count()
                }
            });
    }
}
