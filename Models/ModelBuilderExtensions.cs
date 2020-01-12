using CoreDemo1.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Models
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>().HasData(
                new Employee
                {
                    Id = 1,
                    Name = "Nitish Kumar Verma",
                    Email = "abc@gmail.com",
                    Department = Department.IT
                },
                new Employee
                {
                    Id = 2,
                    Name = "Rajesh",
                    Email = "Rajesh@gg.com",
                    Department = Department.HR
                }
                );
        }
    }
}
