using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoorController : MonoBehaviour
{
    public List<Button> doorButtons = new List<Button>(); // ボタンリスト
    public List<DoorMove> doors = new List<DoorMove>();    // ドアリスト

    private void Start()
    {
        for (int i = 0; i < doorButtons.Count; i++)
        {
            int index = i; // ローカルキャプチャ
            doorButtons[i].onClick.AddListener(() => ToggleLock(index));
        }
    }

    private void ToggleLock(int doorIndex)
    {
        if (doorIndex < 0 || doorIndex >= doors.Count) return;

        doors[doorIndex].Lock = !doors[doorIndex].Lock; // ロック状態を反転
        Debug.Log($"Door {doorIndex} Lock = {doors[doorIndex].Lock}");
    }
}
