using System;
using System.Collections.Generic;

namespace ConsoleApp3
{
    class Program
    {
        static void Main(string[] args)
        {
            // bruh
            List<Employee> employees = new List<Employee>();
            employees.Add(new Employee("John", 1500, "Developer"));
            employees.Add(new Employee("Dylan", 1700, "Developer"));
            employees.Add(new Employee("Tony", 3500, "Manager"));
            employees.Add(new Employee("Christopher", 9999, "Director"));

            ITarget target = new EmployeeAdapter();
            target.AdaptProcessSalaries(employees);

            Console.WriteLine("");
            // Director tries to acces SHARED FOLDER >> Access granted
            SharedFolderProxy sharedFolder1 = new SharedFolderProxy(employees[3]);
            sharedFolder1.PerformOperation(employees[3], employees);

            Console.WriteLine("");
            // Developer tries to access SHARED FOLDER >> Access denied
            SharedFolderProxy sharedFolder2 = new SharedFolderProxy(employees[0]);
            sharedFolder2.PerformOperation(employees[0], employees);
        }
    }

    public class Employee
    {
        public static int ID = 0;
        public string Name { get; set; }
        public int Salary { get; set; }
        public string Role { get; set; }

        public Employee(string name, int salary, string role)
        {
            ID++;
            Name = name;
            Salary = salary;
            Role = role;
        }
    }

    public class ThirdPartySalaryCalculator
    {
        public int ProcessSalaries(int[] salaries)
        {
            int totalSalary = 0;
            for(int i=0; i<salaries.Length; i++)
            {
                totalSalary += salaries[i];
            }

            return totalSalary;
        }
    }

    public interface ITarget
    {
        void AdaptProcessSalaries(List<Employee> employees);
    }

    public class EmployeeAdapter : ITarget
    {
        ThirdPartySalaryCalculator thirdParty = new ThirdPartySalaryCalculator();

        public void AdaptProcessSalaries(List<Employee> employees)
        {
            int[] employeeSalaries = new int[employees.Count];

            for(int i=0; i<employeeSalaries.Length; i++)
            {
                employeeSalaries[i] = employees[i].Salary;
            }

            Console.WriteLine($"Total amount of salary of the company: {thirdParty.ProcessSalaries(employeeSalaries)}");
        }
    }

    public interface ISharedFolder
    {
        void PerformOperation(Employee employee, List<Employee> employees);
    }

    public class SharedFolder : ISharedFolder
    {
        public void PerformOperation(Employee employee, List<Employee> employees)
        {
            Console.WriteLine("The list of all employees. Choose one to modifiy their salary: ");
            for(int i=0; i<employees.Count; i++)
            {
                Console.WriteLine($"{i}. Name: {employees[i].Name} Salary: {employees[i].Salary} Role: {employees[i].Role}");
            }
            int option = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine($"{employees[option].Name} has the salary of: {employees[option].Salary}.\n To what value you want to change: ");
            int toWhatValue = Convert.ToInt32(Console.ReadLine());
            employees[option].Salary = toWhatValue;

            Console.WriteLine($"{employees[option].Name}'s salary was changed to: {employees[option].Salary}");

        }
    }

    public class SharedFolderProxy : ISharedFolder
    {
        private ISharedFolder folder;
        private Employee sharedEmployee;

        public SharedFolderProxy(Employee employee)
        {
            this.sharedEmployee = employee;
        }

        public void PerformOperation(Employee employee, List<Employee> employees)
        {
            if(employee.Role == "Director" || employee.Role == "Manager")
            {
                folder = new SharedFolder();
                folder.PerformOperation(employee, employees);
            }
            else Console.WriteLine($"Your role is {employee.Role}. Proxy Shared Folder does not allow user with such role acces the folder.");
        }

    }
}
