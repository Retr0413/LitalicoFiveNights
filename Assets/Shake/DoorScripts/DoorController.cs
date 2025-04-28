using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoorController : MonoBehaviour
{
    public List<Button> doorButtons = new List<Button>();
    public List<MonoBehaviour> doorComponents = new List<MonoBehaviour>();
    public BatteryUI batteryUI; // BatteryUI参照

    private List<IDoor> doors = new List<IDoor>();
    private bool isBatteryDead = false; // バッテリー切れフラグ

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
            doorButtons[i].onClick.AddListener(() => ToggleLock(index));
        }
    }

    private void Update()
    {
        CheckBatteryState();
        UpdateButtonColors();
    }

    private void CheckBatteryState()
    {
        if (batteryUI == null) return;

        if (batteryUI.BatteryPercentage <= 0f)
        {
            if (!isBatteryDead)
            {
                isBatteryDead = true;
                ForceUnlockAllDoors();
                SetAllButtonsInteractable(false);
            }
        }
        else
        {
            if (isBatteryDead)
            {
                isBatteryDead = false;
                SetAllButtonsInteractable(true);
            }
        }
    }

    private void ForceUnlockAllDoors()
    {
        foreach (var door in doors)
        {
            door.Lock = false;
        }
        Debug.LogWarning("[DoorController] バッテリー切れ → 全ドア強制ロック解除！");
    }

    private void SetAllButtonsInteractable(bool interactable)
    {
        foreach (var button in doorButtons)
        {
            button.interactable = interactable;
        }
    }

    public void ToggleLock(int doorIndex)
    {
        if (doorIndex < 0 || doorIndex >= doors.Count) return;

        if (batteryUI != null && batteryUI.BatteryPercentage <= 0f)
        {
            IDoor door = doors[doorIndex];
            door.Lock = false;
            Debug.LogWarning($"[DoorController] バッテリー切れのため Door {doorIndex} をLockできませんでした（強制解除）！");
            return;
        }

        IDoor normalDoor = doors[doorIndex];
        normalDoor.Lock = true;
        Debug.Log($"[ToggleLock] Door {doorIndex} is now Lock = {normalDoor.Lock}");

        StartCoroutine(AutoUnlock(normalDoor, doorIndex));
    }

    private IEnumerator AutoUnlock(IDoor door, int doorIndex)
    {
        yield return new WaitForSeconds(unlockDelay);

        if (batteryUI != null && batteryUI.BatteryPercentage <= 0f)
        {
            // バッテリー切れなら解除しない（もう解除されてるはず）
            yield break;
        }

        door.Lock = false;
        Debug.Log($"[AutoUnlock] Door {doorIndex} is now Lock = {door.Lock}");
    }

    private void UpdateButtonColors()
    {
        for (int i = 0; i < doors.Count; i++)
        {
            if (i < doorButtons.Count)
            {
                ColorBlock colors = doorButtons[i].colors;

                if (doors[i].Lock)
                {
                    colors.normalColor = Color.green;
                    colors.highlightedColor = Color.green;
                }
                else
                {
                    colors.normalColor = Color.white;
                    colors.highlightedColor = Color.white;
                }

                doorButtons[i].colors = colors;
            }
        }
    }
}
