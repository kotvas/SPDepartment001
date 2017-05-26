using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SPDepartment001.Models.DB
{
    public static class SeedEmployeeData
    {
        public static void EnsurePopulated(IApplicationBuilder app)
        {
            ApplicationDbContext context = app.ApplicationServices
                .GetRequiredService<ApplicationDbContext>();

            AddEmployee(context, GetEmployee("Aleksey", "Galatsan", new DateTime(2000, 7, 7)));
            AddEmployee(context, GetEmployee("Alex", "Kucherenko", new DateTime(2000, 9, 23)));
            AddEmployee(context, GetEmployee("Alexander", "Smirnov", new DateTime(2000, 6, 20)));
            AddEmployee(context, GetEmployee("Alexandra", "Volohova", new DateTime(2000, 5, 6)));
            AddEmployee(context, GetEmployee("Andrey", "Simonenko", new DateTime(2000, 6, 14)));
            AddEmployee(context, GetEmployee("Anna", "Kuzochkina", new DateTime(2000, 12, 22)));
            AddEmployee(context, GetEmployee("Anna", "Skaletskaya", new DateTime(2000, 8, 12)));
            AddEmployee(context, GetEmployee("Artem", "Ivchenko", new DateTime(2000, 8, 11)));
            AddEmployee(context, GetEmployee("Daria", "Morgunova", new DateTime(2000, 8, 8)));
            AddEmployee(context, GetEmployee("Denis", "Smaglyuk", new DateTime(2000, 6, 26)));
            AddEmployee(context, GetEmployee("Dmitriy", "Temnohud", new DateTime(2000, 7, 27)));
            AddEmployee(context, GetEmployee("Dmitry", "Zadorozhniy", new DateTime(2000, 1, 26)));
            AddEmployee(context, GetEmployee("Elena", "Zabiyako", new DateTime(2000, 7, 20)));
            AddEmployee(context, GetEmployee("Eugene", "Polyakov", new DateTime(2000, 8, 22)));
            AddEmployee(context, GetEmployee("Eugeniy", "Polozhenkov", new DateTime(2000, 10, 29)));
            AddEmployee(context, GetEmployee("Evgeniy", "Omelchenko", new DateTime(2000, 11, 30)));
            AddEmployee(context, GetEmployee("Evgeny", "Fedorov", new DateTime(2000, 9, 11)));
            AddEmployee(context, GetEmployee("Jane", "Osipova", new DateTime(2000, 4, 18)));
            AddEmployee(context, GetEmployee("Lyudmila", "Panteyenko", new DateTime(2000, 8, 26)));
            AddEmployee(context, GetEmployee("Maksim", "Neustroev", new DateTime(2000, 7, 22)));
            AddEmployee(context, GetEmployee("Nastja", "Yanush", new DateTime(2000, 9, 27)));
            AddEmployee(context, GetEmployee("Natalia", "Lepsheeva", new DateTime(2000, 6, 17)));
            AddEmployee(context, GetEmployee("Oleg", "Lyashuk", new DateTime(2000, 6, 16)));
            AddEmployee(context, GetEmployee("Olga", "Oprishenko", new DateTime(2000, 11, 24)));
            AddEmployee(context, GetEmployee("Sergey", "Hrabrov", new DateTime(2000, 2, 4)));
            AddEmployee(context, GetEmployee("Sergio", "Blajko", new DateTime(2000, 6, 12)));
            AddEmployee(context, GetEmployee("Sergio", "Tkachenko", new DateTime(2000, 6, 17)));
            AddEmployee(context, GetEmployee("Victoria", "Mel", new DateTime(2000, 8, 23)));
            AddEmployee(context, GetEmployee("Vitaliy", "Gorobets", new DateTime(2000, 10, 5)));
            AddEmployee(context, GetEmployee("Vitaliy", "Kucherenko", new DateTime(2000, 4, 10)));
            AddEmployee(context, GetEmployee("Yegor", "Pustovit", new DateTime(2000, 8, 17)));
            AddEmployee(context, GetEmployee("Yurii", "Levadnyi", new DateTime(2000, 9, 25)));
        }

        private static void AddEmployee(ApplicationDbContext context, Employee employee)
        {
            if (!context.Employees.Any(e => EmployeeExists(e, employee)))
            {
                context.Employees.Add(employee);
                context.SaveChanges();
            }
        }

        private static bool EmployeeExists(Employee dbEmployee, Employee newEmployee)
        {
            if (dbEmployee.FirstName == newEmployee.FirstName &&
                dbEmployee.LastName == newEmployee.LastName &&
                dbEmployee.DateOfBirth == newEmployee.DateOfBirth)
            {
                return true;
            }
            return false;
        }

        private static Employee GetEmployee(string firstName, string lastName, DateTime dtBirth)
            => new Employee() { FirstName = firstName, LastName = lastName, DateOfBirth = dtBirth };
    }
}
