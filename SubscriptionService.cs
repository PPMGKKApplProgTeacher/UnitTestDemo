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
