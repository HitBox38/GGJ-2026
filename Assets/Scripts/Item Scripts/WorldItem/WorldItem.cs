using UnityEngine;

public class WorldItem : MonoBehaviour
{
    [Header("Item Data")]
    [SerializeField] private ItemData _itemData;

    [Header("Components")]
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Collider2D _collider2D;
    [SerializeField] private Rigidbody2D _rigidbody2D;

    private bool _isPickedUp = false;

    public ItemData GetItemData => _itemData;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // initialize item data
        if (_itemData != null)
        {
            Initialize(_itemData);
        }   
    }

    private void Initialize(ItemData itemData)
    {
        this._itemData = itemData;
        // set sprite renderer sprite to item data icon
        if (_spriteRenderer != null && _itemData.itemIcon != null)
        {
            _spriteRenderer.sprite = _itemData.itemIcon;
        }

        // adjust weight by setting rigidbody2D mass
        if (_rigidbody2D != null)
        {
            _rigidbody2D.mass = _itemData.weight;
        }
    }

    public void PickUp(GameObject picker)
    {
        if (!_isPickedUp)
        {
            _isPickedUp = true;
            // disable collider and rigidbody
            if (_collider2D != null && _rigidbody2D != null)
            {
                _rigidbody2D.isKinematic = true;
                _collider2D.enabled = false;
            }

            // set parent to picker
            this.transform.SetParent(picker.transform);
            // set position to picker position
            this.transform.localPosition = Vector3.zero;
        }
    }

    public void Drop()
    {
        if (_isPickedUp)
        {
            _isPickedUp = false;
            // enable collider and rigidbody
            if (_collider2D != null && _rigidbody2D != null)
            {
                _rigidbody2D.isKinematic = false;
                _collider2D.enabled = true;
            }

            // unparent the item
            this.transform.SetParent(null);
        }
    }

    // TODO: better highlight effect
    public void SetHighlight(bool active)
    {
        if (active)
        {
            _spriteRenderer.color = Color.yellow; // simple placeholder highlight effect
        }
        else
        {
            _spriteRenderer.color = Color.white;
        }
    }

    public float GetWeight()
    {
        return _itemData.weight;
    }
}
