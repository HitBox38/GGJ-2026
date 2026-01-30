using System;
using UnityEngine;

public class StopRune : MonoBehaviour
{
    // TODO: check player inventory weight and the max weight vars and stop movement until reaches under 50% of max weight
    private void OnTriggerStay2D(Collider2D other)
    {
        throw new NotImplementedException();
        // if (!other.CompareTag("Player")) return;
        // other.GetComponent<PlayerMovement>().SetSpeedModifier(0);
    }
}
