using UnityEngine;

public class ItemBox : MonoBehaviour
{
    public static ItemBox instance;
    private void Awake()
    {
        instance = this;
    }

    public void SetItem(Item item)
    {
        Debug.Log("Item set: " + item.type);
    }
}
