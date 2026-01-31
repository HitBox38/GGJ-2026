using UnityEngine;

public class ObjectScanner : MonoBehaviour
{
    // radius within which to scan for objects
    [SerializeField] private float scanRadius = 2f;

    // array to hold detected colliders
    [SerializeField] private Collider2D[] detectedColliders;
    // Update is called once per frame
    void Update()
    {
        // scan for nearby objects with colliders
        detectedColliders = Physics2D.OverlapCircleAll(transform.position, scanRadius);
    }

    Collider2D[] GetDetectedColliders(LayerMask layerMask)
    {
        // @TODO: potentially centrelize logic for scanning items 
        return null;
    }
}
