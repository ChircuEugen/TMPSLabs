using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Student jenya = new Student("Jenya", "Kirk", 2113);

            IProffesor profProg = new ProgammingProffesor("Vanya", "Beliy", 1223);
            profProg.Teach(jenya);


            IProffesor profMath = new MathProffesor("Maxim", "Randomsky", 1856);
            profMath.Teach(jenya);

            Console.WriteLine();

            Console.WriteLine($"{jenya.Name} programming skills: {jenya.knowledgeProgramming}");
            Console.WriteLine($"{jenya.Name} math skills: {jenya.knowledgeMaths}");

            Console.WriteLine();

            ProgrammingLaborant progLab = new ProgrammingLaborant("Mihail", "Relescu", 887);

            progLab.Teach(jenya);
            progLab.EvaluateLab(jenya, 0, 9);

            Console.WriteLine($"Nota la laboratorului 1: {jenya.programmingGrades[0]}");
            Console.WriteLine($"{jenya.Name} programming skills: {jenya.knowledgeProgramming}");



            Console.ReadKey();
        }
    }

    public interface IProffesor
    {
        void Teach(Student stud);
    }

    public interface ILaborant
    {
        void EvaluateLab(Student stud, int index, int grade);
    }




    public class Person
    {
        public string Name { get; set; }
        public string Surname { get; set; }

        public Person() { }
        public Person(string name, string surname)
        {
            Name = name;
            Surname = surname;
        }
    }

    public class Student : Person
    {
        public int id;
        public int knowledgeMaths = 0;
        public int knowledgeProgramming = 0;
        public bool isExpelled = false;

        public int[] programmingGrades = new int[4];
        public int[] mathGrades = new int[4];

        public Student() { }
        public Student(string name, string surname, int ID)
        {
            Name = name;
            Surname = surname;
            id = ID;
        }

        //public void Learning(Proffesor prof)
        //{
        //    prof.Teach(this);
        //}
    }

    public class Proffesor : Person
    {
        public string email;
        public string phoneNumber;
        public int id;

    }

    public class ProgammingProffesor : Proffesor, IProffesor
    {
        public ProgammingProffesor() { }
        public ProgammingProffesor(string name, string surname, int ID)
        {
            Name = name;
            Surname = surname;
            id = ID;
        }
        public void Teach(Student stud)
        {
            Console.WriteLine($"Proffesor {Name} is teaching {stud.Name} programming");
            stud.knowledgeProgramming += 2;
        }
    }

    public class MathProffesor : Proffesor, IProffesor
    {
        public MathProffesor() { }
        public MathProffesor(string name, string surname, int ID)
        {
            Name = name;
            Surname = surname;
            id = ID;
        }
        public void Teach(Student stud)
        {
            Console.WriteLine($"Proffesor {Name} is teaching {stud.Name} math");
            stud.knowledgeMaths += 2;
        }
    }

    public class ProgrammingLaborant : Proffesor, IProffesor, ILaborant
    {

        public ProgrammingLaborant(string name, string surname, int ID)
        {
            Name = name;
            Surname = surname;
            id = ID;
        }
        public void Teach(Student stud)
        {
            stud.knowledgeProgramming++;
        }
        
        public void EvaluateLab(Student stud, int index, int grade)
        {
            stud.programmingGrades[index] = grade;
        }
    }

    public class MathLaborant : Proffesor, IProffesor, ILaborant
    {

        public MathLaborant(string name, string surname, int ID)
        {
            Name = name;
            Surname = surname;
            id = ID;
        }
        public void Teach(Student stud)
        {
            stud.knowledgeMaths++;
        }

        public void EvaluateLab(Student stud, int index, int grade)
        {
            stud.mathGrades[index] = grade;
        }
    }

}
