using UnityEngine;

public enum ItemType
{
    Equipment,
    Potion,
    Material
}

[CreateAssetMenu(menuName = "Game/Item")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public ItemType type;
    public Sprite icon;

    public bool stackable = false;
    public int maxStack = 1;
}
