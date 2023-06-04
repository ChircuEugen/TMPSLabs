using System;
using System.Collections.Generic;
using System.Globalization;

namespace Patterns
{
    public enum FuelType { Petrol, Diesel, Gas };
    class Program
    {
        static void Main(string[] args)
        {
            CultureInfo.CurrentCulture = new CultureInfo("en-US", false);


            Director director = new Director();

            CarBuilder builder1 = new FordBuilder();
            CarBuilder builder2 = new HennesseyBuilder();

            director.Construct(builder1);
            Car car1 = builder1.GetResult();
            Console.WriteLine("Builder1 made a Ford car with properties: ");
            car1.Show();

            Console.WriteLine();

            director.Construct(builder2);
            Car car2 = builder2.GetResult();
            Console.WriteLine("Builder2 made a Hennessey car with properties: ");
            car2.Show();

            Console.WriteLine();

            Car car3 = car1.Clone() as Car;
            Console.WriteLine("car3 was clonned from car1 with the same properties: ");
            car3.Show();

            CarCollection carCollection = new CarCollection();
            carCollection.AddCar(car1);
            carCollection.AddCar(car2);
            carCollection.AddCar(car3);

            //Console.WriteLine();
            //for (Car car = carCollection.iter.First(); !carCollection.iter.IsCompleted; car = carCollection.iter.Next())
            //{
            //    car.Show();
            //}

            Iterator iterator = carCollection.CreateIterator();

            Console.WriteLine("\nShowing all elements from carCollection through iterator: ");

            for (Car car = iterator.First(); !iterator.IsCompleted; car = iterator.Next())
            {
                car.Show();
            }

            Console.WriteLine();
            Car testCar = carCollection.GetCar(1);
            testCar.Drive();
            testCar.Drive();
            testCar.Drive();
            testCar.Drive();

            Console.WriteLine();
            testCar.FixCar();

            testCar.Drive();
            testCar.Drive();
            testCar.Drive();

            IAdapter adapter = new CarsWorthAdapter();
            adapter.AdaptProcessPrices(carCollection);

            List<Employee> employees = new List<Employee>();
            employees.Add(new Employee("John", "Drent", "Shop Assistant"));
            employees.Add(new Employee("Dylan", "Kemol", "Shop Assistant"));
            employees.Add(new Employee("Tony", "Grower", "Manager"));
            employees.Add(new Employee("Christopher", "Bestall", "Director"));

            // Director tries to acces SHARED FOLDER >> Access granted
            Console.WriteLine("\nDirector: ");
            SharedFolderProxy sharedFolderProxy1 = new SharedFolderProxy(employees[3]);
            sharedFolderProxy1.PerformOperation(carCollection, iterator);


            // Shop Assistant tries to access SHARED FOLDER >> Access denied
            Console.WriteLine("Shop Assistant: ");
            SharedFolderProxy sharedFolderProxy2 = new SharedFolderProxy(employees[0]);
            sharedFolderProxy2.PerformOperation(carCollection, iterator);


        }
    }

    // Prototype
    public abstract class Prototype
    {
        public abstract Prototype Clone();
    }
    // Prototype
    public class Car : Prototype
    {
        public string name;
        public string model;
        public DateTime releaseDate;
        public int wheels;
        public float price;

        public FuelType fuel;

        public IDrive carState = new FixedState();
        private int timesUntillBroken = 0;

        public int years = 0;

        public Car() { }
        public Car(string name, string model, DateTime releaseDate, int wheels, FuelType fuel, float price)
        {
            this.name = name;
            this.model = model;
            this.releaseDate = releaseDate;
            this.wheels = wheels;
            this.fuel = fuel;
            this.price = price;
            //carState = new FixedState();
        }

        public override Prototype Clone()
        {
            return new Car(name, model, releaseDate, wheels, fuel, price);
        }

        public void Drive()
        {
            carState.Drive();
            timesUntillBroken++;

            if (timesUntillBroken == 3)
            {
                carState = new BrokenState();
                timesUntillBroken = 0;
            }
        }

        public void FixCar()
        {
            if (carState is BrokenState)
            {
                carState = new FixedState();
                Console.WriteLine("Car has been fixed");
            }
            else Console.WriteLine("Car is not broken");
        }

        public void Show()
        {
            //Console.WriteLine($"{this.name} {this.model} {this.releaseDate.Year} {this.releaseDate.AddYears(2)} {this.wheels} {this.fuel} {this.price}");
            Console.WriteLine($"{this.name} {this.model} {this.releaseDate.AddYears(years)} {this.wheels} {this.fuel} {this.price}");
        }


    }

    // Builder
    public abstract class CarBuilder
    {
        public abstract void SetName();
        public abstract void SetModel();
        public abstract void DateOfRelease();
        public abstract void SetNumberOfWheels();
        public abstract void SetFuelType();
        public abstract void SetPrice();
        public abstract Car GetResult();
    }

    public class FordBuilder : CarBuilder
    {
        private Car _car = new Car();

        public override void SetName()
        {
            _car.name = "Ford";
        }
        public override void SetModel()
        {
            _car.model = "Mustang";
        }
        public override void DateOfRelease()
        {
            _car.releaseDate = DateTime.Now;
        }
        public override void SetNumberOfWheels()
        {
            _car.wheels = 4;
        }
        public override void SetFuelType()
        {
            _car.fuel = FuelType.Diesel;
        }
        public override void SetPrice()
        {
            _car.price = 133700.00f;
        }
        public override Car GetResult()
        {
            return _car;
        }
    }

    public class HennesseyBuilder : CarBuilder
    {
        private Car _car = new Car();

        public override void SetName()
        {
            _car.name = "Hennessey";
        }
        public override void SetModel()
        {
            _car.model = "VelociRaptor";
        }
        public override void DateOfRelease()
        {
            _car.releaseDate = DateTime.Now;
        }
        public override void SetNumberOfWheels()
        {
            _car.wheels = 6;
        }
        public override void SetFuelType()
        {
            _car.fuel = FuelType.Petrol;
        }

        public override void SetPrice()
        {
            _car.price = 322200.55f;
        }
        public override Car GetResult()
        {
            return _car;
        }
    }

    public class Director
    {
        public void Construct(CarBuilder builder)
        {
            builder.SetName();
            builder.SetModel();
            builder.DateOfRelease();
            builder.SetNumberOfWheels();
            builder.SetPrice();
            builder.SetFuelType();
        }
    }
    // Builder

    // Iterator
    interface ICollection
    {
        Iterator CreateIterator();
    }

    public class CarCollection : ICollection
    {
        private List<Car> listCars = new List<Car>();

        public Iterator CreateIterator()
        {
            return new Iterator(this);
        }

        public int Count
        {
            get { return listCars.Count; }
        }

        public void AddCar(Car car)
        {
            listCars.Add(car);
        }

        public Car GetCar(int IndexPosition)
        {
            return listCars[IndexPosition];
        }
    }

    interface IIterator
    {
        Car First();
        Car Next();
        bool IsCompleted { get; }
    }

    public class Iterator : IIterator
    {
        private CarCollection collection;
        public int current = 0;
        private readonly int step = 1;

        public Iterator(CarCollection collection)
        {
            this.collection = collection;
        }

        public Car First()
        {
            current = 0;
            return collection.GetCar(current);
        }

        public Car Next()
        {
            current += step;
            if (!IsCompleted)
            {
                return collection.GetCar(current);
            }
            else
            {
                return null;
            }
        }

        public bool IsCompleted
        {
            get { return current >= collection.Count; }
        }
    }
    // Iterator

    // State
    public interface IDrive
    {
        void Drive();
    }
    public class BrokenState : IDrive
    {
        public void Drive()
        {
            Console.WriteLine("This car cannot drive. It is broken");
        }
    }

    public class FixedState : IDrive
    {
        public void Drive()
        {
            Console.WriteLine("Car is driving");
        }
    }
    // State

    // Adapter
    public class ThirdPartyCarsWorth
    {
        public float ProcessPrices(List<Car> cars)
        {
            float totalWorth = 0f;

            for (int i = 0; i < cars.Count; i++)
            {
                totalWorth += cars[i].price;
            }

            return totalWorth;
        }
    }

    public interface IAdapter
    {
        void AdaptProcessPrices(CarCollection carsCollection);
    }

    public class CarsWorthAdapter : IAdapter
    {
        ThirdPartyCarsWorth thirdParty = new ThirdPartyCarsWorth();

        public void AdaptProcessPrices(CarCollection cars)
        {
            List<Car> listCars = new List<Car>();

            for(int i=0; i<cars.Count; i++)
            {
                listCars.Add(cars.GetCar(i));
            }

            Console.WriteLine($"Total car worth is: {thirdParty.ProcessPrices(listCars)}");
        }
    }
    // Adapter

    // Proxy
    public class Employee
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Role { get; set; }

        public Employee(string name, string surname, string role)
        {
            Name = name;
            Surname = surname;
            Role = role;
        }
    }

    interface ISharedFolder
    {
        void PerformOperation(CarCollection cars, Iterator iter);
    }

    public class SharedFolder : ISharedFolder
    {
        public void PerformOperation(CarCollection cars, Iterator iter)
        {
            Console.WriteLine("The list of all cars and their properties: \n");
            for (Car car = iter.First(); !iter.IsCompleted; car = iter.Next())
            {
                Console.Write(iter.current + ". ");
                car.Show();
            }
            Console.WriteLine("Choose the car wanted: ");
            int option = Convert.ToInt32(Console.ReadLine());
            Car carToOperate = cars.GetCar(option);


            while (option != 9)
            {
                Console.WriteLine("\n1. Change Price: ");
                Console.WriteLine("2. Change Year of Release: ");
                Console.WriteLine("9. Exit");
                Console.WriteLine("What operation you want to perform on that specific car: ");

                option = Convert.ToInt32(Console.ReadLine());
                if (option == 1)
                {
                    Console.WriteLine("What price you want to set: ");
                    float newPrice = float.Parse(Console.ReadLine());
                    carToOperate.price = newPrice;
                }
                else if (option == 2)
                {
                    Console.WriteLine("What year of release you want to set: ");
                    int newYear = Convert.ToInt32(Console.ReadLine());
                    int yearDifference = newYear - carToOperate.releaseDate.Year;
                    carToOperate.years = yearDifference;
                }
                Console.WriteLine("\nCar info with changed properties: ");
                carToOperate.Show();
            }
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

        public void PerformOperation(CarCollection cars, Iterator iter)
        {
            if (sharedEmployee.Role == "Director" || sharedEmployee.Role == "Manager")
            {
                folder = new SharedFolder();
                folder.PerformOperation(cars, iter);
            }
            else Console.WriteLine($"Your role is {sharedEmployee.Role}. Proxy Shared Folder does not allow user with such role acces the folder.");
        }
    }
    // Proxy
}
