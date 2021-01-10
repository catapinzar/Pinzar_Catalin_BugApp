using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pinzar_Catalin_BugApp.Models
{
    public class App
    {
        public int AppID { get; set; }
        public int BugID { get; set; }
        public int EmployeeID { get; set; }

        public Bug Bug { get; set; }
        public Employee Employee { get; set; }

    }
}
