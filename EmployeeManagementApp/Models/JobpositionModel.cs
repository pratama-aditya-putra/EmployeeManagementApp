using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Models
{
    public class JobpositionModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Job Name is required")]
        public string JobName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal Salary { get; set; }
        public bool IsActive { get; set; }
        public int EmployeeId { get; set; } // Foreign key
        public EmployeeModel Employee { get; set; } // Navigation property

        public JobpositionModel()
        {

        }
    }
}
