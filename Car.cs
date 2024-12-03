using System;

namespace CarManager
{
    public class Car
    {
        private string make;

        private string model;

        private double fuelConsumption;

        private double fuelAmount;

        private double fuelCapacity;

        private Car()
        {
            this.FuelAmount = 0;
        }

        public Car(string make, string model, double fuelConsumption, double fuelCapacity) : this()
        {
            this.Make = make;
            this.Model = model;
            this.FuelConsumption = fuelConsumption;
            this.FuelCapacity = fuelCapacity;
        }

        public string Make
        {
            get
            {
                return this.make;
            }
            private set
            {
                if (String.IsNullOrEmpty(value))
                {
                    throw new ArgumentException("Make cannot be null or empty!");
                }

                this.make = value;
            }
        }

        public string Model
        {
            get
            {
                return this.model;
            }
            private set
            {
                if (String.IsNullOrEmpty(value))
                {
                    throw new ArgumentException("Model cannot be null or empty!");
                }

                this.model = value;
            }
        }

        public double FuelConsumption
        {
            get
            {
                return this.fuelConsumption;
            }
            private set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("Fuel consumption cannot be zero or negative!");
                }

                this.fuelConsumption = value;
            }
        }

        public double FuelAmount
        {
            get
            {
                return this.fuelAmount;
            }
            private set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Fuel amount cannot be negative!");
                }

                this.fuelAmount = value;
            }
        }

        public double FuelCapacity
        {
            get
            {
                return this.fuelCapacity;
            }
            private set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("Fuel capacity cannot be zero or negative!");
                }

                this.fuelCapacity = value;
            }
        }

        public void Refuel(double fuelToRefuel)
        {
            if (fuelToRefuel <= 0)
            {
                throw new ArgumentException("Fuel amount cannot be zero or negative!");
            }

            this.FuelAmount += fuelToRefuel;

            if (this.FuelAmount > this.FuelCapacity)
            {
                this.FuelAmount = this.FuelCapacity;
            }
        }

        public void Drive(double distance)
        {
            double fuelNeeded = (distance / 100) * this.FuelConsumption;

            if (fuelNeeded > this.FuelAmount)
            {
                throw new InvalidOperationException("You don't have enough fuel to drive!");
            }

            this.FuelAmount -= fuelNeeded;
        }
    }
}




using NUnit.Framework;
using System;

namespace CarManager.Tests
{
    [TestFixture]
    public class CarTests
    {
        private Car car;

        [SetUp]
        public void SetUp()
        {
            car = new Car("Toyota", "Corolla", 6.5, 50);
        }

        [Test]
        public void Constructor_ShouldInitializeCarCorrectly()
        {
            // Assert
            Assert.AreEqual("Toyota", car.Make);
            Assert.AreEqual("Corolla", car.Model);
            Assert.AreEqual(6.5, car.FuelConsumption);
            Assert.AreEqual(50, car.FuelCapacity);
            Assert.AreEqual(0, car.FuelAmount);
        }

        [TestCase(null)]
        [TestCase("")]
        public void Constructor_ShouldThrowException_WhenMakeIsInvalid(string invalidMake)
        {
            // Assert
            Assert.Throws<ArgumentException>(() => new Car(invalidMake, "Corolla", 6.5, 50), "Make cannot be null or empty!");
        }

        [TestCase(null)]
        [TestCase("")]
        public void Constructor_ShouldThrowException_WhenModelIsInvalid(string invalidModel)
        {
            // Assert
            Assert.Throws<ArgumentException>(() => new Car("Toyota", invalidModel, 6.5, 50), "Model cannot be null or empty!");
        }

        [TestCase(0)]
        [TestCase(-5)]
        public void Constructor_ShouldThrowException_WhenFuelConsumptionIsInvalid(double invalidFuelConsumption)
        {
            // Assert
            Assert.Throws<ArgumentException>(() => new Car("Toyota", "Corolla", invalidFuelConsumption, 50), "Fuel consumption cannot be zero or negative!");
        }

        [TestCase(0)]
        [TestCase(-10)]
        public void Constructor_ShouldThrowException_WhenFuelCapacityIsInvalid(double invalidFuelCapacity)
        {
            // Assert
            Assert.Throws<ArgumentException>(() => new Car("Toyota", "Corolla", 6.5, invalidFuelCapacity), "Fuel capacity cannot be zero or negative!");
        }

        [TestCase(-1)]
        [TestCase(0)]
        public void Refuel_ShouldThrowException_WhenFuelToRefuelIsInvalid(double invalidFuelToRefuel)
        {
            // Assert
            Assert.Throws<ArgumentException>(() => car.Refuel(invalidFuelToRefuel), "Fuel amount cannot be zero or negative!");
        }

        [Test]
        public void Refuel_ShouldIncreaseFuelAmountCorrectly()
        {
            // Act
            car.Refuel(20);

            // Assert
            Assert.AreEqual(20, car.FuelAmount);
        }

        [Test]
        public void Refuel_ShouldNotExceedFuelCapacity()
        {
            // Act
            car.Refuel(60);

            // Assert
            Assert.AreEqual(car.FuelCapacity, car.FuelAmount);
        }

        [Test]
        public void Drive_ShouldReduceFuelAmountCorrectly()
        {
            // Arrange
            car.Refuel(20);

            // Act
            car.Drive(100); // 100 km at 6.5 L/100 km = 6.5 L

            // Assert
            Assert.AreEqual(13.5, car.FuelAmount);
        }

        [Test]
        public void Drive_ShouldThrowException_WhenFuelIsInsufficient()
        {
            // Arrange
            car.Refuel(5); // 5L is insufficient for 100 km with 6.5L/100 km

            // Assert
            Assert.Throws<InvalidOperationException>(() => car.Drive(100), "You don't have enough fuel to drive!");
        }
    }
}

