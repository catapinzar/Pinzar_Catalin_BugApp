using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using DataAccess = Pinzar_Catalin_BugApp.Data;
using ModelAccess = Pinzar_Catalin_BugApp.Models;


namespace GrpcEmployeeService
{
    public class GrpcCrudService : EmployeeService.EmployeeServiceBase
    {
        private DataAccess.VerificationContext db = null;
        public GrpcCrudService(DataAccess.VerificationContext db)
        {
            this.db = db;
        }
        public override Task<EmployeeList> GetAll(Empty empty, ServerCallContext context)
        {

            EmployeeList el = new EmployeeList();
            var query = from employee in db.Employees
                        select new Employee()
                        {
                            EmployeeId = employee.EmployeeID,
                            UserName = employee.UserName,
                            Position = employee.Position.ToString(),
                            Birthdate = employee.BirthDate.ToString()
                        };
            el.Item.AddRange(query.ToArray());
            return Task.FromResult(el);
        }
        public override Task<Empty> Insert(Employee requestData, ServerCallContext context)
        {
            db.Employees.Add(new ModelAccess.Employee
            {
                EmployeeID = requestData.EmployeeId,
                UserName = requestData.UserName,
                Position = Enum.Parse<ModelAccess.EmployeePosition>(requestData.Position),
                BirthDate = DateTime.Parse(requestData.Birthdate)
            });
            db.SaveChanges();
            return Task.FromResult(new Empty());
        }

        public override Task<Employee> Get(EmployeeId requestData, ServerCallContext context)
        {
            var data = db.Employees.Find(requestData.Id);

            Employee emp = new Employee()
            {
                EmployeeId = data.EmployeeID,
                UserName = data.UserName,
                Position = data.Position.ToString()
            };
            return Task.FromResult(emp);
        }

        public override Task<Empty> Delete(EmployeeId requestData, ServerCallContext context)
        {
            var data = db.Employees.Find(requestData.Id);
            db.Employees.Remove(data);

            db.SaveChanges();
            return Task.FromResult(new Empty());
        }

        public override Task<Employee> Update(Employee requestData, ServerCallContext context)
        {
            db.Employees.Update(new ModelAccess.Employee()
            {
                EmployeeID = requestData.EmployeeId,
                UserName = requestData.UserName,
                Position = Enum.Parse<ModelAccess.EmployeePosition>(requestData.Position),
                BirthDate = DateTime.Parse(requestData.Birthdate)
            });
            db.SaveChanges();
            return Task.FromResult(requestData);
        }
    }
}
