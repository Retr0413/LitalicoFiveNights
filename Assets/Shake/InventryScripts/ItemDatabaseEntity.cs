using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu]
public class ItemDatabaseEntity : ScriptableObject
{
    public List<Item> items = new List<Item>();
}
