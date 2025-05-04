using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class WallManager : MonoBehaviour
{
    [Header("壁を操作するボタン")]
    public List<Button> wallButtons = new List<Button>();

    [Header("WallBlocker スクリプト（MonoBehaviourとして取得）")]
    public List<MonoBehaviour> wallBlockerComponents = new List<MonoBehaviour>();

    [Header("バッテリー参照")]
    public BatteryUI batteryUI;

    private List<IWallBlocker> wallBlockers = new List<IWallBlocker>();
    private bool isBatteryDead = false;

    private void Start()
    {
        foreach (var component in wallBlockerComponents)
        {
            if (component is IWallBlocker blocker)
            {
                wallBlockers.Add(blocker);
            }
            else
            {
                Debug.LogError($"Component {component.name} does not implement IWallBlocker!");
            }
        }
    }

    private void Update()
    {
        CheckBatteryState();
        UpdateWallButtonColors(); // 毎フレーム色を更新
    }

    private void CheckBatteryState()
    {
        if (batteryUI == null) return;

        if (batteryUI.BatteryPercentage <= 0f)
        {
            if (!isBatteryDead)
            {
                isBatteryDead = true;
                ForceUnblockAllWalls();
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

    private void ForceUnblockAllWalls()
    {
        foreach (var wall in wallBlockers)
        {
            wall.Block = false;
        }
        Debug.LogWarning("[WallManager] バッテリー切れ → 全壁ブロック解除！");
    }

    private void SetAllButtonsInteractable(bool interactable)
    {
        foreach (var button in wallButtons)
        {
            button.interactable = interactable;
        }
    }

    public void ToggleWallBlock(int wallIndex)
    {
        if (wallIndex < 0 || wallIndex >= wallBlockers.Count) return;

        if (batteryUI != null && batteryUI.BatteryPercentage <= 0f)
        {
            wallBlockers[wallIndex].Block = false;
            Debug.LogWarning($"[WallManager] バッテリー切れのため Wall {wallIndex} をBlockできませんでした（強制解除）！");
            return;
        }

        wallBlockers[wallIndex].ToggleBlock();
        UpdateWallButtonColors(); // 即時色反映
    }

    public List<bool> GetAllWallBlockStates()
    {
        List<bool> blockStates = new List<bool>();
        foreach (var blocker in wallBlockers)
        {
            blockStates.Add(blocker.Block);
        }
        return blockStates;
    }

    private void UpdateWallButtonColors()
    {
        for (int i = 0; i < wallBlockers.Count; i++)
        {
            if (i < wallButtons.Count && wallButtons[i] != null)
            {
                ColorBlock colors = wallButtons[i].colors;

                if (wallBlockers[i].Block)
                {
                    Color limeGreen = new Color(0.5f, 1f, 0.5f); // 黄緑色
                    colors.normalColor = limeGreen;
                    colors.highlightedColor = limeGreen;
                    colors.selectedColor = limeGreen;
                }
                else
                {
                    colors.normalColor = Color.white;
                    colors.highlightedColor = Color.white;
                    colors.selectedColor = Color.white;
                }

                wallButtons[i].colors = colors;
            }
        }
    }
}
