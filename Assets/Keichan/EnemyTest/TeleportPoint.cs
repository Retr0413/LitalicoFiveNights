using UnityEngine;
using System.Collections.Generic;

public class TeleportPoint : MonoBehaviour
{
    public List<TeleportPoint> neighbors = new List<TeleportPoint>();
    [SerializeField] private DoorMove nearestDoor; // 最寄りのドア
    [SerializeField] private WallBlocker nearestWallBlocker; // 最寄りの壁

    // エディタ上での可視化
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position, 0.3f);

        Gizmos.color = Color.yellow;
        foreach (var neighbor in neighbors)
        {
            if (neighbor != null)
            {
                Gizmos.DrawLine(transform.position, neighbor.transform.position);
            }
        }
    }

    public bool IsNearestObstaclesClosed(){
        if (nearestDoor == null) return false;
        if (nearestWallBlocker == null) return false;
        return nearestDoor.Lock || nearestWallBlocker.Block;
    }
}
