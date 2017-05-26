using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SPDepartment001.Models.Data
{
    public interface IEmployeeAccountRepository
    {
        IEnumerable<EmployeeAccount> EmployeesAccounts { get; }
    }
}
