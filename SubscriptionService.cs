using System;
using System.Collections.Generic;

public class SubscriptionService
{
    private readonly List<Subscription> _subscriptions = new List<Subscription>();

    // Pricing tiers (simulating a simple in-memory "database")
    private static readonly Dictionary<string, decimal> PricingTiers = new Dictionary<string, decimal>
    {
        { "Basic", 9.99m },
        { "Standard", 19.99m },
        { "Premium", 29.99m }
    };

    public SubscriptionService()
    {
    }

    public bool SubscribeUser(string userEmail, string tier, DateTime startDate)
    {
        if (string.IsNullOrEmpty(userEmail) || !PricingTiers.ContainsKey(tier))
            throw new ArgumentException("Invalid email or tier.");

        var endDate = CalculateSubscriptionEndDate(startDate, tier);
        var subscription = new Subscription
        {
            UserEmail = userEmail,
            Tier = tier,
            StartDate = startDate,
            EndDate = endDate
        };

        // Save the subscription to the list
        _subscriptions.Add(subscription);

        // Simulate sending a notification (no external service needed)
        Console.WriteLine($"[Notification] Sent to {userEmail}: Welcome to the {tier} plan!");

        return true;
    }

    public Subscription GetSubscription(string userEmail)
    {
        if (string.IsNullOrEmpty(userEmail))
            throw new ArgumentException("User email cannot be null or empty.");

        var subscription = _subscriptions.Find(s => s.UserEmail == userEmail);

        if (subscription == null)
            throw new InvalidOperationException("Subscription not found.");

        return subscription;
    }

    public bool CancelSubscription(string userEmail)
    {
        var subscription = _subscriptions.Find(s => s.UserEmail == userEmail);

        // If subscription doesn't exist or is already expired, return false
        if (subscription == null || subscription.EndDate < DateTime.Now)
            return false;

        // Simulate cancellation notification
        Console.WriteLine($"[Notification] Sent to {userEmail}: Your subscription has been cancelled.");

        // Remove the subscription
        _subscriptions.Remove(subscription);

        return true;
    }

    private DateTime CalculateSubscriptionEndDate(DateTime startDate, string tier)
    {
        var subscriptionLength = tier switch
        {
            "Basic" => 1,    // 1 month for Basic
            "Standard" => 3, // 3 months for Standard
            "Premium" => 12, // 12 months for Premium
            _ => throw new ArgumentException("Invalid tier.")
        };

        return startDate.AddMonths(subscriptionLength);
    }
}

public class Subscription
{
    public string UserEmail { get; set; }
    public string Tier { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}


using NUnit.Framework;
using System;

[TestFixture]
public class SubscriptionServiceTests
{
    private SubscriptionService _subscriptionService;

    [SetUp]
    public void SetUp()
    {
        _subscriptionService = new SubscriptionService();
    }

    [Test]
    public void SubscribeUser_ShouldThrowArgumentException_WhenEmailIsInvalid()
    {
        // Arrange
        string invalidEmail = string.Empty;
        string tier = "Standard";
        DateTime startDate = DateTime.Now;

        // Act & Assert
        Assert.Throws<ArgumentException>(() => _subscriptionService.SubscribeUser(invalidEmail, tier, startDate));
    }

    [Test]
    public void SubscribeUser_ShouldThrowArgumentException_WhenTierIsInvalid()
    {
        // Arrange
        string userEmail = "user@example.com";
        string invalidTier = "Gold"; // Invalid tier
        DateTime startDate = DateTime.Now;

        // Act & Assert
        Assert.Throws<ArgumentException>(() => _subscriptionService.SubscribeUser(userEmail, invalidTier, startDate));
    }

    [Test]
    public void SubscribeUser_ShouldCreateSubscription_WhenValidEmailAndTier()
    {
        // Arrange
        string userEmail = "user@example.com";
        string tier = "Premium";
        DateTime startDate = DateTime.Now;

        // Act
        var result = _subscriptionService.SubscribeUser(userEmail, tier, startDate);

        // Assert
        Assert.IsTrue(result);
        var subscription = _subscriptionService.GetSubscription(userEmail);
        Assert.AreEqual(userEmail, subscription.UserEmail);
        Assert.AreEqual(tier, subscription.Tier);
        Assert.AreEqual(startDate.AddMonths(12), subscription.EndDate);
    }

    [Test]
    public void GetSubscription_ShouldThrowInvalidOperationException_WhenSubscriptionDoesNotExist()
    {
        // Arrange
        string nonExistentEmail = "nonexistent@example.com";

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => _subscriptionService.GetSubscription(nonExistentEmail));
    }

    [Test]
    public void CancelSubscription_ShouldReturnFalse_WhenSubscriptionIsExpired()
    {
        // Arrange
        string userEmail = "user@example.com";
        _subscriptionService.SubscribeUser(userEmail, "Standard", DateTime.Now.AddMonths(-2));

        // Act
        var result = _subscriptionService.CancelSubscription(userEmail);

        // Assert
        Assert.IsFalse(result);
    }

    [Test]
    public void CancelSubscription_ShouldReturnTrue_WhenSubscriptionIsActive()
    {
        // Arrange
        string userEmail = "user@example.com";
        _subscriptionService.SubscribeUser(userEmail, "Standard", DateTime.Now.AddMonths(-1));

        // Act
        var result = _subscriptionService.CancelSubscription(userEmail);

        // Assert
        Assert.IsTrue(result);
        Assert.Throws<InvalidOperationException>(() => _subscriptionService.GetSubscription(userEmail)); // Ensure the subscription is removed
    }

    [Test]
    public void CancelSubscription_ShouldRemoveSubscription_WhenCancelled()
    {
        // Arrange
        string userEmail = "user@example.com";
        _subscriptionService.SubscribeUser(userEmail, "Standard", DateTime.Now.AddMonths(-1));

        // Act
        _subscriptionService.CancelSubscription(userEmail);

        // Assert
        Assert.Throws<InvalidOperationException>(() => _subscriptionService.GetSubscription(userEmail)); // Subscription should be removed
    }
}
