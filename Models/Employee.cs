using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pinzar_Catalin_BugApp.Models
{
    public class Employee
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int EmployeeID { get; set; }
        public string UserName { get; set; }
        public EmployeePosition Position { get; set; }
        public DateTime BirthDate { get; set; }

        public ICollection<App> Apps { get; set; }
    }
}
