using System;
using System.Collections;
using UnityEngine;

public class DizzyingCauldronHazard : MonoBehaviour
{
    [SerializeField, Range(0.1f, 0.8f)] private float effectDelay = 0.5f;
    
    private IEnumerator OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) yield break;
        // wait for a bit so that the player wont get stuck in switching directions every frame.
        yield return new WaitForSeconds(effectDelay);
        // multiplying by -1 to invert the speed/inputs
        other.GetComponent<PlayerMovement>().SetSpeedModifier(-1);
    }

    private IEnumerator OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) yield break;
        // wait for a bit so that the player wont get stuck in switching directions every frame.
        yield return new WaitForSeconds(effectDelay);
        // return the speed modifier to normal
        other.GetComponent<PlayerMovement>().SetSpeedModifier(1);
    }
}
