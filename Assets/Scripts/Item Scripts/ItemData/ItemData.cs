using UnityEngine;

[CreateAssetMenu(fileName = "New Item Data", menuName = "Items/Item Data")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public Sprite itemIcon;
    public float value;
    public float weight;
    public float itemRadius;
}
