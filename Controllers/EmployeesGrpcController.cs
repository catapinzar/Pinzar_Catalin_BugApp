using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Grpc.Net.Client;
using GrpcEmployeeService;

namespace Pinzar_Catalin_BugApp.Controllers
{
    public class EmployeesGrpcController : Controller
    {
        private readonly GrpcChannel channel;
        public EmployeesGrpcController()
        {
            channel = GrpcChannel.ForAddress("https://localhost:5001");
        }
        [HttpGet]
        public IActionResult Index()
        {
            var client = new EmployeeService.EmployeeServiceClient(channel);
            EmployeeList employee = client.GetAll(new Empty());
            return View(employee);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Employee employee)
        {
            if (ModelState.IsValid)
            {
                var client = new
                EmployeeService.EmployeeServiceClient(channel);
                var createdEmployee = client.Insert(employee);
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var client = new EmployeeService.EmployeeServiceClient(channel);
            Employee employee = client.Get(new EmployeeId() { Id = (int)id });
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var client = new EmployeeService.EmployeeServiceClient(channel);
            Empty response = client.Delete(new EmployeeId()
            {
                Id = id
            });
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var client = new EmployeeService.EmployeeServiceClient(channel);
            Employee employee = client.Get(new EmployeeId() { Id = (int)id });
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }
        [HttpPost]
        public IActionResult Edit(int id, Employee employee)
        {
            if (id != employee.EmployeeId)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                var client = new EmployeeService.EmployeeServiceClient(channel);
                Employee response = client.Update(employee);
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }
    }
}
