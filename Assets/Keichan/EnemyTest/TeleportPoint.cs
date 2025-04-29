using UnityEngine;
using System.Collections.Generic;

public class TeleportPoint : MonoBehaviour
{
    public List<TeleportPoint> neighbors = new List<TeleportPoint>();

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
}
