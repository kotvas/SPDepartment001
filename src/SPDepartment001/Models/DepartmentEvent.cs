using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SPDepartment001.Models
{
    public class DepartmentEvent
    {
        public Guid Id { get; set; }

        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateCreated { get; set; } = DateTime.Now;

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateOfEvent { get; set; }

        public decimal AmountOfEmployee { get; set; } = 75;

        public bool AreExpensesGenerated { get; set; } = false;

        public string Description { get; set; } = "";
    }
}
