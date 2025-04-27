using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    public static ItemDatabase instance;

    public void Awake()
    {
        instance = this;
    }

    [SerializeField] ItemDatabaseEntity itemDatabaseEntity = default;
    public Item Spawn(Item.Type type)
    {
        for (int i = 0; i < itemDatabaseEntity.items.Count; i++)
        {
            Item itemData = itemDatabaseEntity.items[i];
            if (itemData.type == type)
            {
                return new Item(itemData);
            }
        }
        return null;
    }
}
