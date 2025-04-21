using UnityEngine;

public class WallManager : MonoBehaviour
{
    [System.Serializable]
    public class Wall
    {
        public Transform wallTransform;
        public Vector3 closedOffset = new Vector3(0, 5, 0); // 閉じる時に上に5動かす
        [HideInInspector]
        public Vector3 initialPosition;
        [HideInInspector]
        public bool isClosed = false;
    }

    public Wall[] walls;  // 壁を複数管理する

    private void Start()
    {
        foreach (var wall in walls)
        {
            if (wall.wallTransform != null)
            {
                wall.initialPosition = wall.wallTransform.position;
            }
        }
    }

    // 指定した壁を閉じたり開いたりする
    public void ToggleWall(int wallIndex)
    {
        if (wallIndex < 0 || wallIndex >= walls.Length) return;

        var wall = walls[wallIndex];
        if (wall.isClosed)
        {
            wall.wallTransform.position = wall.initialPosition;
            wall.isClosed = false;
        }
        else
        {
            wall.wallTransform.position = wall.initialPosition + wall.closedOffset;
            wall.isClosed = true;
        }
    }
}
