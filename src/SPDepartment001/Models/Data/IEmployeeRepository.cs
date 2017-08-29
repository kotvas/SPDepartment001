using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SPDepartment001.Models.Data
{
    public interface IEmployeeRepository
    {
        IEnumerable<Employee> ActiveEmployees { get; }
        IEnumerable<Employee> AllEmployees { get; }
        void SaveEmployee(Employee employee);
        Employee DeleteEmployee(int employeeId);
    }
}
