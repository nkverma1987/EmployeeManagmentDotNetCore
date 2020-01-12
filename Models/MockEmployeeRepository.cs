using EmployeeManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreDemo1.Models
{
    public class MockEmployeeRepository : IEmployeeRpository
    {
        private List<Employee> _employeeList;
        public MockEmployeeRepository()
        {
            this._employeeList = new List<Employee>()
            {
                new Employee{Id=1,Name="name1",Email="email1",Department= Department.HR },
                new Employee{Id=2,Name="name2",Email="email2",Department=Department.IT },
                new Employee{Id=3,Name="name3",Email="email3",Department=Department.Payroll }
            };
        }

        public Employee Add(Employee employee)
        {
            employee.Id = _employeeList.Max(i => i.Id) + 1;
            _employeeList.Add(employee);
            return employee;
        }

        public Employee Delete(int id)
        {
            Employee employee = _employeeList.SingleOrDefault(i => i.Id == id);
            if (employee != null)
                _employeeList.Remove(employee);
            return employee;
        }

        public Employee GetEmployee(int id)
        {
            return _employeeList.FirstOrDefault(e => e.Id == id);
        }

        public IEnumerable<Employee> GetEmployees()
        {
            return _employeeList;
        }

        public Employee Update(Employee employeeChanges)
        {
            Employee employee = _employeeList.SingleOrDefault(i => i.Id == employeeChanges.Id);
            if (employee != null)
            {
                employee.Name = employeeChanges.Name;
                employee.Email = employeeChanges.Email;
                employee.Department = employeeChanges.Department;
            }
            return employee;
        }
    }
}
