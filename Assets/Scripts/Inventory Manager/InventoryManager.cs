using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class InventoryManager : MonoBehaviour
{
    // Keep track of inventory items, capacity, more(?)
    [SerializeField] public float maxCarryWeight = 50f;
    [field: SerializeField] private List<WorldItem> inventoryItems {get;} = new List<WorldItem>();
    [SerializeField] public float currentCarryWeight = 0f;

    // Keep track of an item in the inventory for drop
    [SerializeField] private int _selectedItemIndex = 0;

    private PlayerActions _playerActions;

    public static event Action<ItemData[]> OnInventoryChanged;
    
    private void Awake()
    {
        _playerActions = new PlayerActions();
    }

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
            // notify inventory change for score calculation
            OnInventoryChanged?.Invoke(
                ScoreCalculatorHelper.ConvertWorldItemsToData(inventoryItems)
                );
            return true;
        }
        return false;
    }

    public void RemoveItem(InputAction.CallbackContext context)
    {   
        // make sure index is valid and there is an item to remove
        if (inventoryItems.Count == 0) return;
        if (inventoryItems[_selectedItemIndex] != null)
        {
            WorldItem item = inventoryItems[_selectedItemIndex];
            // release the item from the player
            item.Drop();
            // remove item from inventory and update carry weight
            inventoryItems.Remove(item);
            currentCarryWeight -= item.GetWeight();
            // fix selected index
            if (inventoryItems.Count == 0)  
            {
                _selectedItemIndex = 0;
            }                                                                      
            else if (_selectedItemIndex >= inventoryItems.Count)                                                           
            {
                _selectedItemIndex = Mathf.Clamp(_selectedItemIndex, 0,    
                                                inventoryItems.Count - 1);
            }
            // notify inventory change for score calculation
            OnInventoryChanged?.Invoke(
                ScoreCalculatorHelper.ConvertWorldItemsToData(inventoryItems)
            );
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


    private void OnEnable()
    {
        // enable the action map
        _playerActions.KeyboardControls.Enable();

        // subscribe to the InventoryCycle and Drop events
        _playerActions.KeyboardControls.InventoryCycle.performed += OnInventoryCycle;
        _playerActions.KeyboardControls.Drop.performed += RemoveItem;
    }

    private void OnDisable()
    {
        // disable the action map
        _playerActions.KeyboardControls.Disable();

        // unsubscribe from the InventoryCycle and Drop events
        _playerActions.KeyboardControls.InventoryCycle.performed -= OnInventoryCycle;
        _playerActions.KeyboardControls.Drop.performed -= RemoveItem;
    }

    private void OnInventoryCycle(InputAction.CallbackContext context)
    {
        // return if no items in inventory
        if (inventoryItems.Count == 0) return;

        // if cycling positive, move to next item, else previous
        float cycleValue = context.ReadValue<float>();
        if (cycleValue > 0)
        {
            _selectedItemIndex = (_selectedItemIndex + 1) % inventoryItems.Count;
        }
        else if (cycleValue < 0)
        {
            _selectedItemIndex = (_selectedItemIndex - 1 + inventoryItems.Count) % inventoryItems.Count;
        }
    }

}
