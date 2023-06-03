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
        public int timesUntillBroken = 0;

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
            Console.WriteLine($"{this.name} {this.model} {this.releaseDate.Year} {this.wheels} {this.fuel} {this.price}");
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
        private int current = 0;
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
}
