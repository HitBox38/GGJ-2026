using UnityEngine;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{
    // Keep track of inventory items, capacity, more(?)
    [SerializeField] public float maxCarryWeight = 50f;
    [field: SerializeField] private List<WorldItem> inventoryItems {get;}= new List<WorldItem>();
    [SerializeField] private float currentCarryWeight = 0f;

    void Start()
    {
        // count current carry weight based on starting items
        if (inventoryItems.Count > 0)
        {
            foreach (WorldItem item in inventoryItems)
            {
                currentCarryWeight += item.GetWeight();
            }
        }
    }

    /// <summary>
    /// Add item to inventory, should only be called after checking capacity with CanCarryItem
    /// </summary>
    /// <param name="item">world item to add</param>
    public bool AddItem(WorldItem item)
    {   
        if (CanCarryItem(item))
        {     
            // add item to inventory and update carry weight
            inventoryItems.Add(item);
            currentCarryWeight += item.GetWeight();
            return true;
        }
        return false;
    }

    public void RemoveItem(WorldItem item)
    {   
        if (inventoryItems.Contains(item))
        {
            inventoryItems.Remove(item);
            currentCarryWeight -= item.GetWeight();
        }
    }

    /// <summary>
    /// Check if the player can carry the given item based on weight capacity
    /// </summary>
    /// <param name="item"></param>
    /// <returns>True if the item can be carried, false otherwise</returns>
    public bool CanCarryItem(WorldItem item)
    {
        if (item != null)
        {
            return (currentCarryWeight + item.GetWeight()) <= maxCarryWeight;
        }
        return false;
    }

}
