using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoorController : MonoBehaviour
{
    public List<Button> doorButtons = new List<Button>();
    public List<MonoBehaviour> doorComponents = new List<MonoBehaviour>();
    public BatteryUI batteryUI; // BatteryUI参照

    private List<IDoor> doors = new List<IDoor>();
    private bool isBatteryDead = false;

    private void Start()
    {
        // IDoor の実装を抽出
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

        // ボタンイベント登録
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
            doors[doorIndex].Lock = false;
            Debug.LogWarning($"[DoorController] バッテリー切れのため Door {doorIndex} をLockできませんでした（強制解除）！");
            return;
        }

        doors[doorIndex].Lock = !doors[doorIndex].Lock;
        Debug.Log($"[ToggleLock] Door {doorIndex} is now Lock = {doors[doorIndex].Lock}");

        UpdateButtonColors(); // 色更新
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
                    Color limeGreen = new Color(0.5f, 1f, 0.5f); // 黄緑色
                    colors.normalColor = limeGreen;
                    colors.highlightedColor = limeGreen;
                    colors.pressedColor = limeGreen;
                    colors.selectedColor = limeGreen;
                }
                else
                {
                    colors.normalColor = Color.white;
                    colors.highlightedColor = Color.white;
                    colors.pressedColor = Color.white;
                    colors.selectedColor = Color.white;
                }

                doorButtons[i].colors = colors;
            }
        }
    }
}
