using UnityEngine;
using UnityEngine.InputSystem;

public class ObjectPickUp : MonoBehaviour
{
    private PlayerActions _playerActions;
    private WorldItem _closestWorldItem;

    [SerializeField] private LayerMask itemLayer;
    [SerializeField] public float _pickUpRange = 2f;

    private void Awake()
    {
        // Initalize new player actions instance
       _playerActions = new PlayerActions();
    }

    public void Update()
    {
        // scan for nearby interactable objects
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, _pickUpRange, itemLayer);

        // find closest object
        Collider2D closestCollider = null;
        float closestDistance = Mathf.Infinity;

        foreach (var collider in hitColliders)
        {
            float distance = Vector2.Distance(transform.position, collider.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestCollider = collider;
            }
        }
        // check if closest collider is already closest world item
        if (closestCollider != null)
        {
            if (closestCollider.gameObject != _closestWorldItem?.gameObject)
            {
                // Unhighlight the previous closest world item if it exists
                if (_closestWorldItem != null)
                {
                    _closestWorldItem.SetHighlight(false);
                }
                // Set the new closest world item
                _closestWorldItem = closestCollider.GetComponent<WorldItem>();
                if (_closestWorldItem != null)
                {
                    _closestWorldItem.SetHighlight(true);
                }
            }
        }
        else
        {
            // No nearby items found, unhighlight previous closest world item
            if (_closestWorldItem != null)
            {
                _closestWorldItem.SetHighlight(false);
                _closestWorldItem = null;
            }
        }
    }

    private void OnEnable()
    {
        // enable the action map
        _playerActions.KeyboardControls.Enable();

        // subscribe to the Interact event
        _playerActions.KeyboardControls.Interact.performed += OnInteract;
    }

    private void OnDisable()
    {
        // disable the action map
        _playerActions.KeyboardControls.Disable();

        // unsubscribe from the Interact event
        _playerActions.KeyboardControls.Interact.performed -= OnInteract;
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        if (_closestWorldItem != null)
        {
            Debug.Log($"Picking up {_closestWorldItem.name}");
            // TODO: check that player has capacity to pick up item
            _closestWorldItem.PickUp(this.gameObject);
            
            // Optionally clear the target reference immediately since it's now picked up
            // (The Update loop will likely clear it next frame anyway as it moves with player)
            _closestWorldItem.SetHighlight(false);
            _closestWorldItem = null;

        }
    }
}
