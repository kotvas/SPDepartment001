using SPDepartment001.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SPDepartment001.Models.Data
{
    public class EmployeeAccountRepository : IEmployeeAccountRepository
    {
        private ApplicationDbContext context;

        public EmployeeAccountRepository(ApplicationDbContext ctx)
        {
            context = ctx;
        }

        public IEnumerable<EmployeeAccount> EmployeesAccounts => context.EmployeesAccounts;
    }
}
