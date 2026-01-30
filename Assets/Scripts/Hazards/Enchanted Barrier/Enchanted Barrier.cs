using System;
using UnityEngine;

public class EnchantedBarrier : MonoBehaviour
{
    [SerializeField, Range(0, 1)] private float speedReducingPercentage = 0.5f;

    private float _originalMovementSpeed;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        // TODO: half movement speed, integrate with player controls
        if (other.CompareTag("Player"))
        {
            // other.GetComponent<PlayerMovement>();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // TODO: restore movement speed, integrate with player controls
        if (other.CompareTag("Player"))
        {
            // other.GetComponent<PlayerMovement>();
        }
    }
}
