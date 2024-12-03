using System;

public class Dough
{
    private const double BaseCaloriesPerGram = 2.0;

    private static readonly double[] FlourModifiers = { 1.5, 1.0 }; // White, Wholegrain
    private static readonly double[] BakingModifiers = { 0.9, 1.1, 1.0 }; // Crispy, Chewy, Homemade

    private string flourType;
    private string bakingTechnique;
    private double weight;

    public Dough(string flourType, string bakingTechnique, double weight)
    {
        this.FlourType = flourType;
        this.BakingTechnique = bakingTechnique;
        this.Weight = weight;
    }

    public string FlourType
    {
        get => this.flourType;
        private set
        {
            if (value != "White" && value != "Wholegrain")
            {
                throw new ArgumentException("Invalid type of dough.");
            }
            this.flourType = value;
        }
    }

    public string BakingTechnique
    {
        get => this.bakingTechnique;
        private set
        {
            if (value != "Crispy" && value != "Chewy" && value != "Homemade")
            {
                throw new ArgumentException("Invalid type of dough.");
            }
            this.bakingTechnique = value;
        }
    }

    public double Weight
    {
        get => this.weight;
        private set
        {
            if (value < 1 || value > 200)
            {
                throw new ArgumentException("Dough weight should be in the range [1..200].");
            }
            this.weight = value;
        }
    }

    public double CalculateCalories()
    {
        double flourModifier = this.FlourType == "White" ? FlourModifiers[0] : FlourModifiers[1];
        double bakingModifier = this.BakingTechnique switch
        {
            "Crispy" => BakingModifiers[0],
            "Chewy" => BakingModifiers[1],
            "Homemade" => BakingModifiers[2],
            _ => throw new ArgumentException("Invalid type of dough.")
        };

        return this.Weight * BaseCaloriesPerGram * flourModifier * bakingModifier;
    }
}


using NUnit.Framework;
using System;

[TestFixture]
public class DoughTests
{
    private Dough dough;

    [SetUp]
    public void SetUp()
    {
        // This runs before each test to ensure a clean state.
        dough = null;
    }

    [Test]
    public void Constructor_ShouldInitializeCorrectly_WithValidData()
    {
        // Arrange
        string flourType = "White";
        string bakingTechnique = "Chewy";
        double weight = 100;

        // Act
        dough = new Dough(flourType, bakingTechnique, weight);

        // Assert
        Assert.AreEqual(flourType, dough.FlourType, "FlourType not set correctly.");
        Assert.AreEqual(bakingTechnique, dough.BakingTechnique, "BakingTechnique not set correctly.");
        Assert.AreEqual(weight, dough.Weight, "Weight not set correctly.");
    }

    [Test]
    public void Constructor_ShouldThrowException_WhenFlourTypeIsInvalid()
    {
        // Arrange
        string invalidFlourType = "Tip500";
        string bakingTechnique = "Chewy";
        double weight = 100;

        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() => new Dough(invalidFlourType, bakingTechnique, weight));
        Assert.AreEqual("Invalid type of dough.", ex.Message);
    }

    [Test]
    public void Constructor_ShouldThrowException_WhenBakingTechniqueIsInvalid()
    {
        // Arrange
        string flourType = "White";
        string invalidBakingTechnique = "Soft";
        double weight = 100;

        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() => new Dough(flourType, invalidBakingTechnique, weight));
        Assert.AreEqual("Invalid type of dough.", ex.Message);
    }

    [Test]
    public void Constructor_ShouldThrowException_WhenWeightIsOutOfRange()
    {
        // Arrange
        string flourType = "White";
        string bakingTechnique = "Chewy";
        double invalidWeight = 240;

        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() => new Dough(flourType, bakingTechnique, invalidWeight));
        Assert.AreEqual("Dough weight should be in the range [1..200].", ex.Message);
    }

    [Test]
    public void CalculateCalories_ShouldReturnCorrectValue()
    {
        // Arrange
        dough = new Dough("White", "Chewy", 100);

        // Act
        double calories = dough.CalculateCalories();

        // Assert
        Assert.AreEqual(330.00, calories, "Calories calculation is incorrect.");
    }

    [TestCase("White", "Crispy", 50, 135.00)] // Valid case 1
    [TestCase("Wholegrain", "Homemade", 150, 300.00)] // Valid case 2
    [TestCase("White", "Chewy", 200, 660.00)] // Valid case 3
    public void CalculateCalories_ShouldWorkForVariousValidData(string flourType, string bakingTechnique, double weight, double expectedCalories)
    {
        // Arrange
        dough = new Dough(flourType, bakingTechnique, weight);

        // Act
        double actualCalories = dough.CalculateCalories();

        // Assert
        Assert.AreEqual(expectedCalories, actualCalories, "Calories calculation for the given input is incorrect.");
    }
}

