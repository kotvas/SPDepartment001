using SPDepartment001.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SPDepartment001.Models.Data
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private ApplicationDbContext context;

        public EmployeeRepository(ApplicationDbContext ctx)
        {
            context = ctx;
        }

        public IEnumerable<Employee> ActiveEmployees => context.Employees.Where(e => e.IsActive);

        public IEnumerable<Employee> AllEmployees => context.Employees;

        public void SaveEmployee(Employee employee)
        {
            if (employee.EmployeeID == 0)
            {
                context.Employees.Add(employee);
            }
            else
            {
                Employee dbEntry = context.Employees
                    .FirstOrDefault(e => e.EmployeeID == employee.EmployeeID);
                if (dbEntry != null)
                {
                    dbEntry.FirstName = employee.FirstName;
                    dbEntry.LastName = employee.LastName;
                    dbEntry.DateOfBirth = employee.DateOfBirth;
                    dbEntry.IsActive = employee.IsActive;
                    dbEntry.AssociatedUserId = employee.AssociatedUserId;
                }
            }
            context.SaveChanges();
        }

        public Employee DeleteEmployee(int employeeID)
        {
            Employee dbEntry = context.Employees
                .FirstOrDefault(e => e.EmployeeID == employeeID);
            if (dbEntry != null)
            {
                context.Employees.Remove(dbEntry);
                context.SaveChanges();
            }
            return dbEntry;
        }
    }
}
