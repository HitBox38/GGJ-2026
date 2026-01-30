using UnityEngine;

public class WorldItem : MonoBehaviour
{
    [Header("Item Data")]
    [SerializeField] private ItemData _itemData;

    [Header("Components")]
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private CircleCollider2D _circleCollider2D;
    [SerializeField] private Rigidbody2D _rigidbody2D;

    private bool _isPickedUp = false;

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

        // adjust circle collider radius
        if (_circleCollider2D != null)
        {
            _circleCollider2D.radius = _itemData.itemRadius;
        }
    }

    public void PickUp(GameObject picker)
    {
        if (!_isPickedUp)
        {
            _isPickedUp = true;
            // disable collider and rigidbody
            if (_circleCollider2D != null && _rigidbody2D != null)
            {
                _rigidbody2D.isKinematic = true;
                _circleCollider2D.enabled = false;
            }

            // set parent to picker
            this.transform.SetParent(picker.transform);
            // set position to picker position
            this.transform.localPosition = Vector3.zero;
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
