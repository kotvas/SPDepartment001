using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SPDepartment001.Models.Data
{
    public interface IDepartmentEventRepository
    {
        IEnumerable<DepartmentEvent> DepartmentEvents { get; }
        void SaveDepartmentEvent(DepartmentEvent departmentEvent);
        DepartmentEvent DeleteDepartmentEvent(Guid departmentEventId);
    }
}
