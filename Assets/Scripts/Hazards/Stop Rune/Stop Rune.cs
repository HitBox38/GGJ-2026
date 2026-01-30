using System;
using UnityEngine;

public class StopRune : MonoBehaviour
{
    [SerializeField] private float maxWeightPercentage = 0.5f;
    
    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        var currentWeightPercentage =
            InventoryWeightHelper.GetWeightPercentage(other.GetComponentInChildren<InventoryManager>());
        if (currentWeightPercentage <= maxWeightPercentage)
        {
            other.GetComponent<PlayerMovement>().SetSpeedModifier(1);
            return;
        }
        other.GetComponent<PlayerMovement>().SetSpeedModifier(0);
    }
}
