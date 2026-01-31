using System;
using UnityEngine;

public class EnchantedBarrier : MonoBehaviour
{
    [SerializeField, Range(0, 1)] private float speedReducingPercentage = 0.5f;

    private float _originalMovementSpeed;
    
    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        other.GetComponent<PlayerMovement>().SetSpeedModifier(speedReducingPercentage);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        // if player has a rabbit mask, set the speed modifier to the mask's modifer
        RabbitMask rabbitMask = other.GetComponentInChildren<RabbitMask>();
        if (rabbitMask != null)
        {
            other.GetComponent<PlayerMovement>().SetSpeedModifier(rabbitMask.speedBoostAmount);
            return;
        }
        other.GetComponent<PlayerMovement>().SetSpeedModifier(1);
    }
}
