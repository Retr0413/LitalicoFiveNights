using UnityEngine;
using System.Collections.Generic;

public class WallManager : MonoBehaviour
{
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
}
