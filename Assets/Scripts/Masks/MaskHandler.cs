using UnityEngine;
using UnityEngine.InputSystem;

public class MaskHandler : MonoBehaviour
{
    public int removeMaskValue = 3;
    [SerializeField] private GameObject player;
    [SerializeField] private MaskObject[] masks;
    [SerializeField] private int currentMaskIndex;

    

    // track the current mask object
    private MaskObject currentMaskInstance;
    private PlayerActions _playerActions;

    private void Awake()
    {
        _playerActions = new PlayerActions();
    }

    private void Start()
    {
        currentMaskIndex = removeMaskValue; // start with no mask equipped
    }

    public void ChangeMask(int newIndex)
    {
        if (newIndex >= 0 && newIndex < masks.Length)
        {
            EquipMask(newIndex);
        }
    }

    private void EquipMask(int index)
    {
        // destroy old instance of mask (if exists)
        if (currentMaskInstance != null)
        {
            Destroy(currentMaskInstance.gameObject);
        }

        // update the index
        currentMaskIndex = index;

        // instantiate new mask and initialize it
        currentMaskInstance = Instantiate(masks[currentMaskIndex], 
                                        player.transform.position, 
                                        Quaternion.identity);

        // TODO: parent the mask to the player (if needed)
        currentMaskInstance.transform.SetParent(player.transform);

        currentMaskInstance.Initialize(player);
        currentMaskInstance.ApplyEffects();

    }

    public void RemoveMask()
    {
        if (currentMaskInstance != null)
        {
            Destroy(currentMaskInstance.gameObject);
            currentMaskInstance = null;
            currentMaskIndex = removeMaskValue;
        }
    }

    private void OnEnable()
    {
        _playerActions.KeyboardControls.EquipMask.performed += SelectMask;
        // _playerActions.KeyboardControls.EquipMask1.performed += SelectMask;
        // _playerActions.KeyboardControls.RemoveMask.performed += SelectMask;
        _playerActions.KeyboardControls.Enable();
    }

    private void OnDisable()
    {
        _playerActions.KeyboardControls.EquipMask.performed -= SelectMask;
        // _playerActions.KeyboardControls.EquipMask1.performed -= SelectMask;
        // _playerActions.KeyboardControls.RemoveMask.performed -= SelectMask;
        _playerActions.KeyboardControls.Disable();
    }

    public void SelectMask(InputAction.CallbackContext context)
    {
        int value = (int)context.ReadValue<float>();
        if (value == removeMaskValue)
        {
            RemoveMask();
        }
        else
        {
            int index = value - 1; // assuming values start at 1
            ChangeMask(index);
        }
    }

    public bool CompareMask(MaskObject mask)
    {
        return currentMaskInstance == mask;
    }

}
