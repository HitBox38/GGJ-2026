using UnityEngine;
using System.Collections.Generic;

public class OxMask : MaskObject
{
    public Sprite oxMaskSprite;
    public float capacityIncrease = 20f;
    private Sprite _originalSprite;
    private SpriteRenderer _playerSpriteRenderer;
    [SerializeField] private InventoryManager _playerInventory = null;

    public class OxStrength : MaskEffect
    {
        public float amountToAdd; 
        public InventoryManager inv;

        public void Initialize(InventoryManager inventory, float amount)
        {
            inv = inventory;
            print("inv == null: " + (inv == null));
            amountToAdd = amount;
            
            if (inv != null)
            {
                print("Increasing max carry weight by " + amountToAdd);
                inv.maxCarryWeight += amountToAdd;
            }
        }
    } 

    public class SwitchWeakness : MaskEffect
    {
        public float capacityPenalty = 0f;
        public InventoryManager inv;

        public void Initialize(InventoryManager inventory, float penalty)
        {
            inv = inventory;
            capacityPenalty = penalty;
        }

        private void OnDestroy()
        {
            // Remove the penalty from max carry weight
            // InventoryManager inv = GetComponent<InventoryManager>();
            if (inv != null)
            {
                inv.maxCarryWeight -= capacityPenalty;
            }

            if (inv != null)
            {
                if (inv.currentCarryWeight > inv.maxCarryWeight)
                {
                    inv.ResetItemIndex();
                    
                    // Access the list using the public property (ensure InventoryManager exposes it)
                    // Note: This relies on inventoryItems being accessible.
                    // If it's private in InventoryManager, that file needs update too.
                    // Using reflection or a public getter would be safer if strict encapsulation is needed.
                    // For now, assuming we will fix InventoryManager to make it public.
                    List<WorldItem> items = inv.inventoryItems; 
                    
                    // remove items until under max carry weight
                    while (inv.currentCarryWeight > inv.maxCarryWeight && items.Count > 0)
                    {
                        WorldItem item = items[items.Count - 1];
                        inv.DropItem(item); 
                    }
                }
            }
        }
    }

    public override void Initialize(GameObject player)
    {
        print("player == null: " + (player == null));
        base.Initialize(player);
        // get the inventory manager
        _playerInventory = player.GetComponentInChildren<InventoryManager>();
        // print("_playerInventory == null: " + (_playerInventory == null));
        // change player sprite to ox mask sprite
        if (oxMaskSprite != null)
        {
            _playerSpriteRenderer = player?.GetComponent<SpriteRenderer>();
            _originalSprite = _playerSpriteRenderer?.sprite;
            _playerSpriteRenderer.sprite = oxMaskSprite;
        }
    }

    public override void ApplyEffects()
    {
        // add the ox strength buff component
        OxStrength strengthComp = AddBuff<OxStrength>();
        // add the switch weakness debuff component
        SwitchWeakness switchComp = AddDebuff<SwitchWeakness>();

        // Configure the buff
        if (strengthComp != null)
        {
            strengthComp.Initialize(_playerInventory, capacityIncrease);
        }

        if (switchComp != null)
        {
            switchComp.Initialize(_playerInventory, capacityIncrease);
        }
    }

    protected override void OnDestroy()
    {
        // restore original sprite
        if (_playerSpriteRenderer != null && _originalSprite != null)
        {
            _playerSpriteRenderer.sprite = _originalSprite;
        }
        base.OnDestroy();
    }
}
