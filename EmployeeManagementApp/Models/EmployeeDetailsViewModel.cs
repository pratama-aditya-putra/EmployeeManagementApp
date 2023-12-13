using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Models
{
    public class EmployeeDetailsViewModel
    {
        public EmployeeModel Employee { get; set; }
        public List<JobpositionModel> JobPositions { get; set; }
    }
}
