using System;

namespace ConsoleApp2
{
    public enum FuelType { Petrol, Diesel, Gas };
    class Program
    {
        static void Main(string[] args)
        {
            //Car car1 = new Car("Ford", "Mustang", new DateTime(2019, 3, 4), 4, FuelType.Gas);
            //Console.WriteLine("Car1's properties: ");
            //Console.WriteLine($"{car1.name} {car1.model} {car1.releaseDate.Year} {car1.wheels} {car1.fuel}");

            //Car car2 = car1.Clone() as Car;

            //Console.WriteLine("Car2's properties: ");
            //Console.WriteLine($"{car2.name} {car2.model} {car2.releaseDate.Year} {car2.wheels} {car2.fuel}");

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
        }
    }

    public abstract class Prototype
    {
        public abstract Prototype Clone();
    }
    public class Car : Prototype
    {
        public string name;
        public string model;
        public DateTime releaseDate;
        public int wheels;

        public FuelType fuel;

        public Car() { }
        public Car(string name, string model, DateTime releaseDate, int wheels, FuelType fuel)
        {
            this.name = name;
            this.model = model;
            this.releaseDate = releaseDate;
            this.wheels = wheels;
            this.fuel = fuel;
        }

        public override Prototype Clone()
        {
            return new Car(name, model, releaseDate, wheels, fuel);
        }

        public void Show()
        {
            Console.WriteLine($"{this.name} {this.model} {this.releaseDate.Year} {this.wheels} {this.fuel}");
        }
    }

    public abstract class CarBuilder
    {
        public abstract void SetName();
        public abstract void SetModel();
        public abstract void DateOfRelease();
        public abstract void SetNumberOfWheels();
        public abstract void SetFuelType();
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
            builder.SetFuelType();
        }
    }

    //public interface ICarBuilder
    //{
    //    public void SetName(string name);
    //    public void SetModel(string model);
    //    public void DateOfRelease(DateTime releaseDate);
    //    public void SetNumberOfWheels(int wheels);
    //    public void SetFuelType(FuelType fuel);
    //}

    //public class CarBuilder : ICarBuilder
    //{
    //    private Car car;
    //    public void SetName(string name) { car.name = name; }
    //    public void SetModel(string model) { car.model = model; }
    //    public void DateOfRelease(DateTime releaseDate) { car.releaseDate = releaseDate; }
    //    public void SetNumberOfWheels(int wheels) { car.wheels = wheels; }

    //    public void SetFuelType(FuelType fuel) { car.fuel = fuel; }

    //    public Car GetCar() { return car; }
    //}
}
