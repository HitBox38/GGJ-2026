using System;
using UnityEngine;

public class EnchantedBarrier : MonoBehaviour
{
    [SerializeField, Range(0, 1)] private float speedReducingPercentage = 0.5f;

    private float _originalMovementSpeed;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        other.GetComponent<PlayerMovement>().SetSpeedModifier(speedReducingPercentage);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        other.GetComponent<PlayerMovement>().SetSpeedModifier(1);
    }
}
