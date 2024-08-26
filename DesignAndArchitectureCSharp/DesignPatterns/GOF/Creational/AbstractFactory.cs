using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignAndArchitectureCSharp.DesignPatterns.GOF.Creational
{
    // Abstract Factory
    // Se usa empleados y departamentos como parte principal, sirve para crear fabricas concretas
    public abstract class EmployeeFactory
    {
        public abstract Employee CreateEmployee(string name, int age);
        public abstract Department CreateDepartment(string name);
    }

    // Concrete Factory 1: Gerentes y Departamento de Recursos Humanos
    public class ManagementFactory : EmployeeFactory
    {
        public override Employee CreateEmployee(string name, int age)
        {
            return new Manager(name, age);
        }

        public override Department CreateDepartment(string name)
        {
            return new HumanResourcesDepartment(name);
        }
    }

    // Concrete Factory 2: Desarrolladores y Departamento de Tecnología
    public class DevelopmentFactory : EmployeeFactory
    {
        public override Employee CreateEmployee(string name, int age)
        {
            return new Developer(name, age);
        }

        public override Department CreateDepartment(string name)
        {
            return new TechnologyDepartment(name);
        }
    }

    // Concrete Factory 3: Diseñadores y Departamento de Marketing
    public class DesignFactory : EmployeeFactory
    {
        public override Employee CreateEmployee(string name, int age)
        {
            return new Designer(name, age);
        }

        public override Department CreateDepartment(string name)
        {
            return new MarketingDepartment(name);
        }
    }
    // Los productos abstractos son lo que serviran como base para productos concretos
    // Abstract Product: Empleado
    public abstract class Employee
    {
        public string Name { get; set; }
        public int Age { get; set; }

        public Employee(string name, int age) => (Name, Age) = (name, age);
    }

    // Abstract Product: Departamento
    public abstract class Department
    {
        public string Name { get; set; }

        public Department(string name) => (Name) = name;
    }

    // Concrete Product 1: Gerente
    public class Manager(string name, int age) : Employee(name, age)
    {
    }

    // Concrete Product 2: Desarrollador
    public class Developer(string name, int age) : Employee(name, age)
    {
    }

    // Concrete Product 3: Diseñador
    public class Designer(string name, int age) : Employee(name, age)
    {
    }

    // Concrete Product 4: Departamento de Recursos Humanos
    public class HumanResourcesDepartment(string name) : Department(name)
    {
    }

    // Concrete Product 5: Departamento de Tecnología
    public class TechnologyDepartment(string name) : Department(name)
    {
    }

    // Concrete Product 6: Departamento de Marketing
    public class MarketingDepartment(string name) : Department(name)
    {
    }
}
