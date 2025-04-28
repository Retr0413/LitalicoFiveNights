using UnityEngine;

public class PickUpObj : MonoBehaviour
{
    public Item.Type type =default;
    public void OnClickObj()
    {
        Item item = ItemDatabase.instance.Spawn(type);
        ItemBox.instance.SetItem(item);
        gameObject.SetActive(false);
    }
}
