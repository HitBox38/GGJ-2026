using UnityEngine;

public abstract class MaskEffect : MonoBehaviour { }

/// <summary>
/// <br>A simple marker class to identify mask objects in the game.</br>
/// <br>This class can be expanded with mask-specific functionality.</br>
/// </summary>
public class MaskObject : MonoBehaviour
{
    [SerializeField] protected private GameObject player;
    // add an empty buff component which specific masks can override
    private Component _buffComponent;
    private Component _debuffComponent;

    public virtual void Initialize(GameObject player)
    {
        // set this player to player
        this.player = player;
        // individual masks can use this method to apply effect to the player
        ApplyEffects();
    }

    public virtual void ApplyEffects()
    {
        // individual masks can implement their own effects here
    }

    // helper methods for adding buff and debuff components
    protected T AddBuff<T>() where T : MaskEffect 
    {
        T component = player.AddComponent<T>();
        _buffComponent = component;
        return component;
    } 

    protected T AddDebuff<T>() where T : MaskEffect 
    {
        T component = player.AddComponent<T>();
        _debuffComponent = component;
        return component;
    }

    protected virtual void OnDestroy()
    {
        // remove any applied effects when the mask is destroyed
        if (_buffComponent != null)
        {
            Destroy(_buffComponent);
        }

        if (_debuffComponent != null)
        {
            Destroy(_debuffComponent);
        }
    }   
}
