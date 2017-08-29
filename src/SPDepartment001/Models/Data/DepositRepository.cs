using Microsoft.EntityFrameworkCore;
using SPDepartment001.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SPDepartment001.Models.Data
{
    public class DepositRepository : IDepositRepository
    {
        private ApplicationDbContext context;

        public DepositRepository(ApplicationDbContext ctx)
        {
            context = ctx;
        }

        public IEnumerable<Deposit> Deposits
            => context.Deposits.Include(d => d.Employee).OrderByDescending(d => d.Date);

        public void SaveDeposit(Deposit deposit)
        {
            if (deposit.Id == new Guid())
            {
                //context.Deposits.Add(deposit);

                Employee employee = context.Employees
                    .Where(empl => empl.EmployeeID == deposit.EmployeeId)
                    .FirstOrDefault();

                EmployeeAccount employeeAccount = context.EmployeesAccounts
                    .Where(eA => eA.EmployeeId == deposit.EmployeeId)
                    .FirstOrDefault();

                using (var dbContextTransaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        context.Deposits.Add(deposit);
                        context.SaveChanges();

                        if (employeeAccount == null)
                        {
                            var newEmployeeAccount = new EmployeeAccount()
                            {
                                EmployeeId = employee.EmployeeID,
                                Employee = employee,
                                Amount = deposit.Amount,
                                DateOfLastUpdate = DateTime.Now
                            };

                            context.EmployeesAccounts.Add(newEmployeeAccount);
                            context.SaveChanges();
                        }
                        else
                        {
                            employeeAccount.Amount = employeeAccount.Amount + deposit.Amount;
                            employeeAccount.DateOfLastUpdate = DateTime.Now;

                            context.SaveChanges();
                        }

                        dbContextTransaction.Commit();
                    }
                    catch
                    {
                        dbContextTransaction.Rollback();
                    }
                }
            }
            else
            {
                ////TODO: Update functionality
                //Deposit dbEntry = context.Deposits
                //    .FirstOrDefault(d => d.Id == deposit.Id);
                //if (dbEntry != null)
                //{
                //    dbEntry.Amount = deposit.Amount;
                //    dbEntry.EmployeeId = deposit.EmployeeId;
                //    dbEntry.Employee = deposit.Employee;
                //}
            }
            context.SaveChanges();
        }

        public Deposit DeleteDeposit(Guid depositID)
        {
            Deposit dbEntry = context.Deposits
                .FirstOrDefault(d => d.Id == depositID);
            if (dbEntry != null)
            {
                EmployeeAccount employeeAccount = context.EmployeesAccounts
                    .Where(eA => eA.EmployeeId == dbEntry.EmployeeId)
                    .FirstOrDefault();

                using (var dbContextTransaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        context.Deposits.Remove(dbEntry);
                        context.SaveChanges();

                        if (employeeAccount != null)
                        {
                            employeeAccount.Amount = employeeAccount.Amount - dbEntry.Amount;
                            employeeAccount.DateOfLastUpdate = DateTime.Now;

                            context.SaveChanges();
                        }

                        dbContextTransaction.Commit();
                    }
                    catch
                    {
                        dbContextTransaction.Rollback();
                    }
                }


                context.SaveChanges();
            }
            return dbEntry;
        }
    }
}
