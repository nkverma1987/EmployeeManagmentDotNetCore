﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreDemo1.Models
{
   public interface IEmployeeRpository
    {
        Employee GetEmployee(int id);
        IEnumerable<Employee> GetEmployees();
        Employee Add(Employee employee);
        Employee Update(Employee employeeChanges);
        Employee Delete(int id);
    }
}
