using UnityEngine;
using System;

[Serializable]
public class Item
{
    public enum Type
    {
        Lamp,
        Monitor,
    }
    public Type type;
    public Item(Item item)
    {
        this.type = item.type;
    }
}
