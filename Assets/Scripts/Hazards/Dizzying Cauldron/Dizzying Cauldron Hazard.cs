using System;
using System.Collections;
using UnityEngine;

public class DizzyingCauldronHazard : MonoBehaviour
{
    [SerializeField, Range(0.1f, 0.8f)] private float effectDelayEnter = 0.5f;
    [SerializeField, Range(0.1f, 0.8f)] private float effectDelayExit = 0.5f;
    
    private IEnumerator OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) yield break;
        // wait for a bit so that the player wont get stuck in switching directions every frame.
        yield return new WaitForSeconds(effectDelayEnter);
        // multiplying by -1 to invert the speed/inputs
        other.GetComponent<PlayerMovement>().SetSpeedModifier(-1);
    }

    private IEnumerator OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) yield break;
        // wait for a bit so that the player wont get stuck in switching directions every frame.
        yield return new WaitForSeconds(effectDelayExit);
        // return the speed modifier to normal
        other.GetComponent<PlayerMovement>().SetSpeedModifier(1);
    }

    public void SetEffectDelayEnter(float delay)
    {
        effectDelayEnter = delay;
    }
    public void SetEffectDelayExit(float delay)
    {
        effectDelayExit = delay;
    }
    public float GetEffectDelayEnter()
    {
        return effectDelayEnter;
    }
    public float GetEffectDelayExit()
    {
        return effectDelayExit; 
    }
}
