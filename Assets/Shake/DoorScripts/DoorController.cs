using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoorController : MonoBehaviour
{
    public List<Button> doorButtons = new List<Button>();
    public List<DoorMove> doors = new List<DoorMove>();

    private void Start()
    {
        for (int i = 0; i < doorButtons.Count; i++)
        {
            int index = i; // ローカルキャプチャが重要！

            doorButtons[i].onClick.AddListener(() =>
            {
                ToggleLock(index);
            });
        }
    }

    public void ToggleLock(int doorIndex)
    {
        if (doorIndex < 0 || doorIndex >= doors.Count) return;

        DoorMove door = doors[doorIndex];
        door.Lock = !door.Lock;

        Debug.Log($"[ToggleLock] Door {doorIndex} is now Lock = {door.Lock}");
    }
}
