using UnityEngine;

public class MaskHandler : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private MaskObject[] masks;
    [SerializeField] private int currentMaskIndex = 0;

    // track the current mask object
    private MaskObject currentMaskInstance;

    private void Start()
    {
        if (masks.Length > 0)
        {
            // Initialize the first mask if exists
            EquipMask(currentMaskIndex);
        }
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

    }

}
