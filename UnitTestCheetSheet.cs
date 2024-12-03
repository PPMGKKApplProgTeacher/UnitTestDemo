public class Calculator
{
    public int Add(int a, int b) => a + b;
    public int Subtract(int a, int b) => a - b;
    public int Multiply(int a, int b) => a * b;
    public int Divide(int a, int b)
    {
        if (b == 0) throw new DivideByZeroException("Cannot divide by zero!");
        return a / b;
    }
}
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace CalculatorTests
{
    [TestFixture]
    [Category("Arithmetic")]
    public class CalculatorTests
    {
        private Calculator calculator;

        [OneTimeSetUp] // Runs once before all tests
        public void GlobalSetup()
        {
            Console.WriteLine("OneTimeSetUp: Initializing test resources...");
        }

        [SetUp] // Runs before each test
        public void SetUp()
        {
            calculator = new Calculator();
        }

        [Test] // Regular test
        public void Add_ShouldReturnCorrectSum()
        {
            Assert.AreEqual(5, calculator.Add(2, 3));
        }

        [TestCase(6, 2, 3)] // Parameterized test
        [TestCase(10, 5, 2)]
        [TestCase(0, 0, 1)]
        public void Divide_ShouldReturnCorrectQuotient(int expected, int a, int b)
        {
            Assert.AreEqual(expected, calculator.Divide(a, b));
        }

        [Test] // Test expecting an exception
        public void Divide_ShouldThrowDivideByZeroException_WhenDivisorIsZero()
        {
            Assert.Throws<DivideByZeroException>(() => calculator.Divide(10, 0));
        }

        [TestCaseSource(nameof(MultiplyTestCases))] // External data source
        public void Multiply_ShouldReturnCorrectProduct(int expected, int a, int b)
        {
            Assert.AreEqual(expected, calculator.Multiply(a, b));
        }

        private static IEnumerable<TestCaseData> MultiplyTestCases()
        {
            yield return new TestCaseData(6, 2, 3).SetName("Multiply_PositiveNumbers");
            yield return new TestCaseData(0, 0, 5).SetName("Multiply_WithZero");
            yield return new TestCaseData(-10, -2, 5).SetName("Multiply_NegativeNumbers");
        }

        [Test]
        [Explicit("This test is run only manually")] // Run explicitly
        public void Subtract_ShouldReturnCorrectDifference()
        {
            Assert.AreEqual(5, calculator.Subtract(10, 5));
        }

        [Test]
        [Ignore("Test temporarily disabled")] // Ignored test
        public void IgnoreExample_ShouldBeSkipped()
        {
            Assert.Fail("This test should not run.");
        }

        [TearDown] // Runs after each test
        public void TearDown()
        {
            calculator = null;
        }

        [OneTimeTearDown] // Runs once after all tests
        public void GlobalTearDown()
        {
            Console.WriteLine("OneTimeTearDown: Cleaning up test resources...");
        }
    }
}

