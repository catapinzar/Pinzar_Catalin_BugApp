using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pinzar_Catalin_BugApp.Models
{
    public class Bug
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public BugSeverity Severity { get; set; }
        public BugStatus Status { get; set; }
        public DateTime DateAdded { get; set; }

        public ICollection<App> Apps { get; set; }
    }
}
