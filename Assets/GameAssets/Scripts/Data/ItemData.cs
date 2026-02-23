using GameAssets.Scripts.Data;
using System;
using Unity.VisualScripting;
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
    [Header("무기일때만")]
    public WeaponData data;
}
