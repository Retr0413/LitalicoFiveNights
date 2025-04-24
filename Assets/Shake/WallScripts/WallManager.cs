using UnityEngine;

public class WallManager : MonoBehaviour
{
    public WallBlocker[] wallBlockers;

    public void ToggleWallBlock(int wallIndex)
    {
        if (wallIndex < 0 || wallIndex >= wallBlockers.Length) return;

        wallBlockers[wallIndex].ToggleBlock();
    }
}
