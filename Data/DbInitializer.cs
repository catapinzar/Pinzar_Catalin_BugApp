using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pinzar_Catalin_BugApp.Models;

namespace Pinzar_Catalin_BugApp.Data
{
    public class DbInitializer
    {
        public static void Initialize(VerificationContext context)
        {
            context.Database.EnsureCreated();
            if (context.Bugs.Any())
            {
                return; // BD a fost creata anterior
            }
            var bugs = new Bug[]
            {
                new Bug{Name="Bug 1", Description="Bug 1 description", Severity=BugSeverity.CRITICAL, Status=BugStatus.Unsolved},
                new Bug{Name="Bug 2", Description="Bug 2 description", Severity=BugSeverity.TRIVIAL, Status=BugStatus.Unsolved},
                new Bug{Name="Bug 3", Description="Bug 3 description", Severity=BugSeverity.MINOR, Status=BugStatus.Solved}
            };
            foreach (Bug bug in bugs)
            {
                context.Bugs.Add(bug);
            }
            context.SaveChanges();

            var employees = new Employee[]
            {
                new Employee{EmployeeID=1050, UserName="tester1", Position=EmployeePosition.Tester, BirthDate=DateTime.Parse("1979-09-01")},
                new Employee{EmployeeID=1045, UserName="programmer1", Position=EmployeePosition.Programmer, BirthDate=DateTime.Parse("1969-07-08")},
            };
            foreach (Employee employee in employees)
            {
                context.Employees.Add(employee);
            }
            context.SaveChanges();

            var apps = new App[]
            {
                new App{BugID=1,EmployeeID=1050},
                new App{BugID=3,EmployeeID=1045},
                new App{BugID=1,EmployeeID=1045},
                new App{BugID=2,EmployeeID=1050},
            };
            foreach (App app in apps)
            {
                context.Apps.Add(app);
            }
            context.SaveChanges();
        }
    }
}
