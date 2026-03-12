using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationCore.Entities;

namespace Infrastructure
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(AppDbContext context)
        {
            if (!context.Employees.Any())
            {
                context.Employees.AddRange(
                    new Employee
                    {
                        LastName = "Ivanov",
                        FirstName = "Ivan",
                        MiddleName = "Ivanovich",
                        Email = "vanya1994@mail.ru",
                    },
                    new Employee
                    {
                        LastName = "Иванов",
                        FirstName = "Иван",
                        MiddleName = "Иванович",
                        Email = "pro100vanya@mail.ru",
                    },
                    new Employee
                    {
                        LastName = "Александров",
                        FirstName = "Александр",
                        MiddleName = "Александрович",
                        Email = "sasha124512@mail.ru",
                    },
                    new Employee
                    {
                        LastName = "Alexandrov",
                        FirstName = "Alexandr",
                        MiddleName = "Alexandrovich",
                        Email = "sanya124214@mail.ru",
                    },
                    new Employee
                    {
                        LastName = "Alexeyev",
                        FirstName = "Alexey",
                        MiddleName = "Alexeyevich",
                        Email = "alex14@rambler.ru",
                    },
                    new Employee
                    {
                        LastName = "Алексеев",
                        FirstName = "Алексей",
                        MiddleName = "Алексеевич",
                        Email = "alex2021@gmail.com",
                    }
                );
            }

            if (!context.Projects.Any())
            {
                context.Projects.AddRange(
                    new Project
                    {
                        Name = "Сделать покушать",
                        Priority = 1,
                        CompanyContractorName = "ООО Я",
                        CompanyExecutorName = "ООО Я",
                        StartDate = DateTime.Today.AddDays(-1).ToUniversalTime(),
                        EndDate = DateTime.Today.AddDays(1).ToUniversalTime(),
                    },
                    new Project
                    {
                        Name = "Make something to eat",
                        Priority = 1,
                        CompanyContractorName = "LLC ME",
                        CompanyExecutorName = "LLC ME",
                        StartDate = DateTime.Today.AddDays(-10).ToUniversalTime(),
                        EndDate = DateTime.Today.AddDays(10).ToUniversalTime(),
                    },
                    new Project
                    {
                        Name = "Build a new API",
                        Priority = 3,
                        CompanyContractorName = "TechCorp",
                        CompanyExecutorName = "DevHouse",
                        StartDate = DateTime.Today.AddDays(-5).ToUniversalTime(),
                        EndDate = DateTime.Today.AddDays(5).ToUniversalTime(),
                    },
                    new Project
                    {
                        Name = "Refactor old system",
                        Priority = 2,
                        CompanyContractorName = "OldSoft",
                        CompanyExecutorName = "CodeMasters",
                        StartDate = DateTime.Today.AddDays(-10).ToUniversalTime(),
                        EndDate = DateTime.Today.AddDays(1).ToUniversalTime(),
                    }
                );
            }

            await context.SaveChangesAsync();
        }
    }
}
