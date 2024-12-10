using System;
using System.Collections.Generic;

public class Warehouse
{
    private readonly Dictionary<string, int> inventory = new();

    public void AddItem(string itemName, int quantity)
    {
        if (quantity <= 0) throw new ArgumentException("Quantity must be positive.");
        if (inventory.ContainsKey(itemName))
            inventory[itemName] += quantity;
        else
            inventory[itemName] = quantity;
    }

    public void RemoveItem(string itemName, int quantity)
    {
        if (!inventory.ContainsKey(itemName)) throw new ArgumentException("Item not found.");
        if (quantity <= 0) throw new ArgumentException("Quantity must be positive.");
        if (inventory[itemName] < quantity) throw new InvalidOperationException("Insufficient stock.");

        inventory[itemName] -= quantity;
        if (inventory[itemName] == 0)
            inventory.Remove(itemName);
    }

    public int CheckStock(string itemName)
    {
        return inventory.TryGetValue(itemName, out int quantity) ? quantity : 0;
    }

    public List<string> ListItems()
    {
        return new List<string>(inventory.Keys);
    }

    public bool HasItem(string itemName)
    {
        return inventory.ContainsKey(itemName);
    }

    public void ClearInventory()
    {
        inventory.Clear();
    }

    public int TotalItems()
    {
        return inventory.Count;
    }

    public int TotalQuantity()
    {
        int totalQuantity = 0;
        foreach (var quantity in inventory.Values)
        {
            totalQuantity += quantity;
        }
        return totalQuantity;
    }
}
