using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using SPDepartment001.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SPDepartment001.Models.DB
{
    public static class SeedEmployeeUserData
    {
        const string DEFAULT_PASSWORD = "Password123!";

        public static void EnsurePopulated(IApplicationBuilder app)
        {
            ApplicationDbContext context = app.ApplicationServices
                .GetRequiredService<ApplicationDbContext>();

            UserManager<AppUser> userManager = app.ApplicationServices
                .GetRequiredService<UserManager<AppUser>>();

            AddEmployee(context, userManager, GetEmployee("Aleksey", "Galatsan", new DateTime(2000, 7, 7)));
            AddEmployee(context, userManager, GetEmployee("Alex", "Kucherenko", new DateTime(2000, 9, 23)));
            AddEmployee(context, userManager, GetEmployee("Alexander", "Smirnov", new DateTime(2000, 6, 20)));
            AddEmployee(context, userManager, GetEmployee("Alexandra", "Volohova", new DateTime(2000, 5, 6)));
            AddEmployee(context, userManager, GetEmployee("Andrey", "Simonenko", new DateTime(2000, 1, 14)));
            AddEmployee(context, userManager, GetEmployee("Anna", "Kuzochkina", new DateTime(2000, 12, 22)));
            AddEmployee(context, userManager, GetEmployee("Anna", "Skaletskaya", new DateTime(2000, 8, 12)));
            AddEmployee(context, userManager, GetEmployee("Artem", "Ivchenko", new DateTime(2000, 8, 11)));
            AddEmployee(context, userManager, GetEmployee("Daria", "Morgunova", new DateTime(2000, 8, 8)));
            AddEmployee(context, userManager, GetEmployee("Denis", "Smaglyuk", new DateTime(2000, 6, 26)));
            AddEmployee(context, userManager, GetEmployee("Dmitriy", "Temnohud", new DateTime(2000, 7, 27)));
            AddEmployee(context, userManager, GetEmployee("Dmitry", "Zadorozhniy", new DateTime(2000, 1, 26)));
            AddEmployee(context, userManager, GetEmployee("Elena", "Zabiyako", new DateTime(2000, 7, 20)));
            AddEmployee(context, userManager, GetEmployee("Eugene", "Polyakov", new DateTime(2000, 8, 22)));
            AddEmployee(context, userManager, GetEmployee("Eugeniy", "Polozhenkov", new DateTime(2000, 10, 29)));
            AddEmployee(context, userManager, GetEmployee("Evgeniy", "Omelchenko", new DateTime(2000, 11, 30)));
            AddEmployee(context, userManager, GetEmployee("Evgeny", "Fedorov", new DateTime(2000, 9, 11)));
            AddEmployee(context, userManager, GetEmployee("Jane", "Osipova", new DateTime(2000, 4, 18)));
            AddEmployee(context, userManager, GetEmployee("Lyudmila", "Panteyenko", new DateTime(2000, 8, 26)));
            AddEmployee(context, userManager, GetEmployee("Maksim", "Neustroev", new DateTime(2000, 7, 22)));
            AddEmployee(context, userManager, GetEmployee("Nastja", "Yanush", new DateTime(2000, 9, 27)));
            AddEmployee(context, userManager, GetEmployee("Natalia", "Lepsheeva", new DateTime(2000, 6, 17)));
            AddEmployee(context, userManager, GetEmployee("Oleg", "Lyashuk", new DateTime(2000, 6, 16)));
            AddEmployee(context, userManager, GetEmployee("Olga", "Oprishenko", new DateTime(2000, 11, 24)));
            AddEmployee(context, userManager, GetEmployee("Sergey", "Hrabrov", new DateTime(2000, 2, 4)));
            AddEmployee(context, userManager, GetEmployee("Sergio", "Blajko", new DateTime(2000, 6, 12)));
            AddEmployee(context, userManager, GetEmployee("Sergio", "Tkachenko", new DateTime(2000, 6, 17)));
            AddEmployee(context, userManager, GetEmployee("Victoria", "Mel", new DateTime(2000, 8, 23)));
            AddEmployee(context, userManager, GetEmployee("Vitaliy", "Gorobets", new DateTime(2000, 10, 5)));
            AddEmployee(context, userManager, GetEmployee("Vitaliy", "Kucherenko", new DateTime(2000, 4, 10)));
            AddEmployee(context, userManager, GetEmployee("Yegor", "Pustovit", new DateTime(2000, 8, 17)));
            AddEmployee(context, userManager, GetEmployee("Yurii", "Levadnyi", new DateTime(2000, 9, 25)));
        }

        private static void AddEmployee(ApplicationDbContext context, UserManager<AppUser> userManager, Employee employee)
        {
            Guid userId = CheckAndCreateUser(userManager, $"{employee.FirstName.First()}.{employee.LastName}").Result;

            if (!context.Employees.Any(e => EmployeeExists(e, employee)))
            {
                employee.AssociatedUserId = userId;
                context.Employees.Add(employee);
                context.SaveChanges();
            }
            else
            {
                Employee existingEmployee = context.Employees.Where(e => e.FirstName == employee.FirstName &&
                e.LastName == employee.LastName &&
                e.DateOfBirth == employee.DateOfBirth).FirstOrDefault();
                existingEmployee.AssociatedUserId = userId;
                context.SaveChanges();
            }


        }

        private static async Task<Guid> CheckAndCreateUser(UserManager<AppUser> userManager, string userName)
        {
            AppUser existingUser = await userManager.FindByNameAsync(userName);

            if (existingUser != null)
            {
                return new Guid(existingUser.Id);
            }

            if (existingUser == null)
            {
                AppUser user = new AppUser
                {
                    UserName = userName,
                    Email = $"{userName}@nixsolutions.com"
                };

                IdentityResult result = await userManager
                    .CreateAsync(user, DEFAULT_PASSWORD);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Users");
                    return new Guid(user.Id);
                }
            }

            return new Guid();
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
