using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoorController : MonoBehaviour
{
    public List<Button> doorButtons = new List<Button>();
    public List<MonoBehaviour> doorComponents = new List<MonoBehaviour>();

    private List<IDoor> doors = new List<IDoor>();

    [Header("ロック解除までの時間設定")]
    public float unlockDelay = 5f;

    private void Start()
    {
        foreach (var component in doorComponents)
        {
            if (component is IDoor door)
            {
                doors.Add(door);
            }
            else
            {
                Debug.LogWarning($"Component {component.GetType().Name} does not implement IDoor.");
            }
        }

        for (int i = 0; i < doorButtons.Count; i++)
        {
            int index = i;

            doorButtons[i].onClick.AddListener(() =>
            {
                ToggleLock(index);
            });
        }
    }

    private void Update()
    {
        UpdateButtonColors(); // ← 毎フレームボタンの色を更新
    }

    public void ToggleLock(int doorIndex)
    {
        if (doorIndex < 0 || doorIndex >= doors.Count) return;

        IDoor door = doors[doorIndex];
        door.Lock = true;

        Debug.Log($"[ToggleLock] Door {doorIndex} is now Lock = {door.Lock}");

        StartCoroutine(AutoUnlock(door, doorIndex));
    }

    private IEnumerator AutoUnlock(IDoor door, int doorIndex)
    {
        yield return new WaitForSeconds(unlockDelay);

        door.Lock = false;
        Debug.Log($"[AutoUnlock] Door {doorIndex} is now Lock = {door.Lock}");
    }

    public List<bool> GetAllDoorLockStates()
    {
        List<bool> lockStates = new List<bool>();

        foreach (var door in doors)
        {
            lockStates.Add(door.Lock);
        }

        return lockStates;
    }

    // 👇 ここでボタンの色を更新する！
    private void UpdateButtonColors()
    {
        for (int i = 0; i < doors.Count; i++)
        {
            if (i < doorButtons.Count)
            {
                ColorBlock colors = doorButtons[i].colors;

                if (doors[i].Lock)
                {
                    // ロック中：ボタンを緑色に
                    colors.normalColor = Color.green;
                    colors.highlightedColor = Color.green;
                }
                else
                {
                    // ロック解除中：ボタンを白に戻す
                    colors.normalColor = Color.white;
                    colors.highlightedColor = Color.white;
                }

                doorButtons[i].colors = colors;
            }
        }
    }
}
