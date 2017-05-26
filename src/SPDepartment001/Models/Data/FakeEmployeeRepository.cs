using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SPDepartment001.Models.Data
{
    public class FakeEmployeeRepository //: IEmployeeRepository
    {
        public IEnumerable<Employee> Employees => new List<Employee>
        {
            new Employee
            {
                FirstName = "FirstName1",
                LastName = "LastName1",
                DateOfBirth = new DateTime(1990,4,18)
            },
            new Employee
            {
                FirstName = "FirstName2",
                LastName = "LastName2",
                DateOfBirth = new DateTime(1991,5,19)
            },
            new Employee
            {
                FirstName = "FirstName3",
                LastName = "LastName3",
                DateOfBirth = new DateTime(1992,6,20)
            }
        };
    }
}
