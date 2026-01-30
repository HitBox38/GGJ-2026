using UnityEngine;
/// <summary>
/// <br>A simple marker class to identify mask objects in the game.</br>
/// <br>This class can be expanded with mask-specific functionality.</br>
/// </summary>
public class MaskObject : MonoBehaviour
{
    [SerializeField] protected private GameObject player;

    public virtual void Initialize(GameObject player)
    {
        // set this player to player
        this.player = player;
        // individual masks can use this method to apply effect to the player
    }

    protected virtual void Update()
    {
        // individual masks can implement their own update logic here
    }
}
