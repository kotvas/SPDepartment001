using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SPDepartment001.Models.DB
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<EmployeeAccount> EmployeesAccounts { get; set; }

        public DbSet<Deposit> Deposits { get; set; }
        public DbSet<DepartmentEvent> DepartmentEvents { get; set; }
        public DbSet<Expense> Expenses { get; set; }

    }
}
