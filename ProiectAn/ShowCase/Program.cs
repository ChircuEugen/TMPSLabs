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


            //Director director = new Director();
            //CarBuilder builder1 = new FordBuilder();
            //CarBuilder builder2 = new HennesseyBuilder();
            CarCollection carCollection = new CarCollection();
            Iterator iterator = carCollection.CreateIterator();
            IAdapter adapter = new CarsWorthAdapter();

            List<Employee> employees = new List<Employee>();
            employees.Add(new Employee("John", "12345", "Shop Assistant"));
            employees.Add(new Employee("Dylan", "qwerty", "Shop Assistant"));
            employees.Add(new Employee("Tony", "manager", "Manager"));
            employees.Add(new Employee("Christopher", "director", "Director"));

            char menuOption = '[';
            while(menuOption != 'e')
            {
                Console.Clear();
                if (menuOption == '1')
                {
                    Director director = new Director();
                    CarBuilder builder1 = new FordBuilder();
                    director.Construct(builder1);
                    carCollection.AddCar(builder1.GetResult());
                    //builder1 = null;
                }

                else if (menuOption == '2')
                {
                    Director director = new Director();
                    CarBuilder builder2 = new HennesseyBuilder();
                    director.Construct(builder2);
                    carCollection.AddCar(builder2.GetResult());
                    //builder2 = null;
                }
                else if(menuOption == '3')
                {
                    PrintCars(iterator);
                    Console.WriteLine("");
                    int cloneOption = Convert.ToInt32(Console.ReadLine());
                    Car optionCar = carCollection.GetCar(cloneOption);
                    Car cloneCar = optionCar.Clone() as Car;
                    carCollection.AddCar(cloneCar);
                }
                else if(menuOption == '4')
                {
                    PrintCars(iterator);
                    Console.WriteLine("Choose the car to drive: ");
                    int driveOption = Convert.ToInt32(Console.ReadLine());
                    Car driveCar = carCollection.GetCar(driveOption);
                    driveCar.Drive();
                }
                else if (menuOption == '5')
                {
                    PrintCars(iterator);
                    Console.WriteLine("Choose a car to fix: ");
                    int fixOption = Convert.ToInt32(Console.ReadLine());
                    Car fixCar = carCollection.GetCar(fixOption);
                    fixCar.FixCar();
                }
                else if(menuOption == '6')
                {
                    PrintCars(iterator);
                }
                else if(menuOption == '7')
                {
                    adapter.AdaptProcessPrices(carCollection);
                }
                else if(menuOption == '8')
                {
                    Console.WriteLine("Log in: ");
                    Console.Write("Login: ");
                    string log = Console.ReadLine();
                    Console.Write("Password: ");
                    string pass = Console.ReadLine();
                    Employee user = null;
                    for(int i=0; i<employees.Count; i++)
                    {
                        if(log == employees[i].Name && pass == employees[i].Password)
                        {
                            user = employees[i];
                            break;
                        }
                    }

                    SharedFolderProxy sharedFolderProxy = new SharedFolderProxy(user);
                    sharedFolderProxy.PerformOperation(carCollection, iterator);
                }

                Console.WriteLine("Menu:");
                Console.WriteLine("1. Build a Ford car");
                Console.WriteLine("2. Build a Hennessey car");
                Console.WriteLine("3. Clone a car from any existent");
                Console.WriteLine("4. Drive a car");
                Console.WriteLine("5. Fix a car");
                Console.WriteLine("6. Show the list of cars");
                Console.WriteLine("7. Print total worth of current cars");
                Console.WriteLine("8. Change car properties (date of release/price)");
                Console.WriteLine("e. Exit");

                menuOption = Console.ReadLine()[0];
            }

        }
        public static void PrintCars(Iterator iterator)
        {
            for (Car car = iterator.First(); !iterator.IsCompleted; car = iterator.Next())
            {
                Console.Write(iterator.current + ". ");
                car.Show();
            }
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

        public IState carState = new FixedState();
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
    public interface IState
    {
        void Drive();
    }
    public class BrokenState : IState
    {
        public void Drive()
        {
            Console.WriteLine("This car cannot drive. It is broken");
        }
    }

    public class FixedState : IState
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

            for (int i = 0; i < cars.Count; i++)
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
        public string Password { get; set; }
        public string Role { get; set; }

        public Employee(string name, string password, string role)
        {
            Name = name;
            Password = password;
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
