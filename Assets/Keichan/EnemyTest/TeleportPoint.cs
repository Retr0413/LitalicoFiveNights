using UnityEngine;
using System.Collections.Generic;

public class TeleportPoint : MonoBehaviour
{
    public List<TeleportPoint> neighbors = new List<TeleportPoint>();
    [SerializeField] private DoorMoveTest nearestDoor; // 最寄りのドア

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

    public bool IsNearestDoorClosed(){
        if (nearestDoor == null) return false;
        return nearestDoor.Lock;
    }
}
