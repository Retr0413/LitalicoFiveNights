using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class WallManager : MonoBehaviour
{
    public List<Button> wallButtons = new List<Button>(); // 壁用ボタンリスト
    public List<MonoBehaviour> wallBlockerComponents = new List<MonoBehaviour>(); // IWallBlockerを持つコンポーネント

    private List<IWallBlocker> wallBlockers = new List<IWallBlocker>();

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
        UpdateWallButtonColors(); // ← 毎フレームボタンの色を更新
    }

    public void ToggleWallBlock(int wallIndex)
    {
        if (wallIndex < 0 || wallIndex >= wallBlockers.Count) return;

        wallBlockers[wallIndex].ToggleBlock();
    }

    // 全WallのBlock状態を取得するメソッド
    public List<bool> GetAllWallBlockStates()
    {
        List<bool> blockStates = new List<bool>();

        foreach (var blocker in wallBlockers)
        {
            blockStates.Add(blocker.Block);
        }

        return blockStates;
    }

    // 👇 ここでボタンの色を更新する
    private void UpdateWallButtonColors()
    {
        for (int i = 0; i < wallBlockers.Count; i++)
        {
            if (i < wallButtons.Count)
            {
                ColorBlock colors = wallButtons[i].colors;

                if (wallBlockers[i].Block)
                {
                    // ブロック中：緑色に
                    colors.normalColor = Color.green;
                    colors.highlightedColor = Color.green;
                }
                else
                {
                    // ブロック解除中：白色に戻す
                    colors.normalColor = Color.white;
                    colors.highlightedColor = Color.white;
                }

                wallButtons[i].colors = colors;
            }
        }
    }
}
