using Microsoft.EntityFrameworkCore;
using SPDepartment001.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SPDepartment001.Models.Data
{
    public class DepartmentEventRepository : IDepartmentEventRepository
    {
        private ApplicationDbContext context;

        public DepartmentEventRepository(ApplicationDbContext ctx)
        {
            context = ctx;
        }

        public IEnumerable<DepartmentEvent> DepartmentEvents
            => context.DepartmentEvents.Include(de => de.Employee).OrderByDescending(de => de.DateCreated);

        public void SaveDepartmentEvent(DepartmentEvent departmentEvent)
        {
            if (departmentEvent.Id == new Guid())
            {
                context.DepartmentEvents.Add(departmentEvent);
            }
            else
            {
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

        public DepartmentEvent DeleteDepartmentEvent(Guid departmentEventId)
        {
            DepartmentEvent dbEntry = context.DepartmentEvents
                .FirstOrDefault(de => de.Id == departmentEventId);
            if (dbEntry != null)
            {
                context.DepartmentEvents.Remove(dbEntry);
                context.SaveChanges();
            }
            return dbEntry;
        }
    }
}
