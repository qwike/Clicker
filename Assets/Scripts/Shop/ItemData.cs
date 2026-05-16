using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public abstract class ItemData : ScriptableObject
{
    public int itemID;
    public string itemName;
    public Sprite icon;
    public int price;

    public string GetKey() => "Item_" + itemID;
}