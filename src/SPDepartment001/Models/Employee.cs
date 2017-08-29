using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SPDepartment001.Models
{
    public class Employee
    {
        public int EmployeeID { get; set; }

        [Required(ErrorMessage = "Please enter a first name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter a last name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please enter a date of birth")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        public bool IsActive { get; set; } = true;

        public Guid AssociatedUserId { get; set; }
    }
}
