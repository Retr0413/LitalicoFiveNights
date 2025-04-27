using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoorController : MonoBehaviour
{
    public List<Button> doorButtons = new List<Button>();
    public List<DoorMove> doors = new List<DoorMove>();

    [Header("ロック解除までの時間設定")]
    public float unlockDelay = 5f; 

    private void Start()
    {
        for (int i = 0; i < doorButtons.Count; i++)
        {
            int index = i; 

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
        door.Lock = true; 

        Debug.Log($"[ToggleLock] Door {doorIndex} is now Lock = {door.Lock}");

        StartCoroutine(AutoUnlock(door, doorIndex));
    }

    private IEnumerator AutoUnlock(DoorMove door, int doorIndex)
    {
        yield return new WaitForSeconds(unlockDelay);

        door.Lock = false; 
        Debug.Log($"[AutoUnlock] Door {doorIndex} is now Lock = {door.Lock}");
    }
}
