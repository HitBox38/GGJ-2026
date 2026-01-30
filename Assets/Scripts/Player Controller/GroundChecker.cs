using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    public bool IsGrounded { get; private set;}
    
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Ground") && !IsGrounded)
        {
            // we are touching the ground!
            IsGrounded = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        IsGrounded = false;
    }
}