using UnityEngine;
using System.Collections.Generic;

public class Pathfinder : MonoBehaviour
{
    static float Heuristic(Vector3 a, Vector3 b)
    {
        return Vector3.Distance(a, b);
    }

    public static List<TeleportPoint> AStar(TeleportPoint start, TeleportPoint goal)
    {
        List<TeleportPoint> openSet = new List<TeleportPoint>();                                  // 未探索リスト
        HashSet<TeleportPoint> closedSet = new HashSet<TeleportPoint>();                          // 探索済みリスト
        Dictionary<TeleportPoint, TeleportPoint> cameFrom = new Dictionary<TeleportPoint, TeleportPoint>(); // 各ノードの前のノード
        Dictionary<TeleportPoint, float> gScore = new Dictionary<TeleportPoint, float>();         // 各ノードからスタートまでのコスト
        Dictionary<TeleportPoint, float> fScore = new Dictionary<TeleportPoint, float>();         // 各ノードのスタートからのコストとヒューリスティックで算出したコスト

        openSet.Add(start);
        gScore[start] = 0;
        fScore[start] = Heuristic(start.transform.position, goal.transform.position);

        while (openSet.Count > 0)
        {
            TeleportPoint current = openSet[0];
            foreach (var point in openSet)
            {
                if (fScore.ContainsKey(point) && fScore[point] < fScore[current])
                {
                    current = point;
                }
            }

            if (current == goal)
            {
                return ReconstructPath(cameFrom, current);
            }

            openSet.Remove(current);
            closedSet.Add(current);

            foreach (var neighbor in current.neighbors)
            {
                if (closedSet.Contains(neighbor))
                    continue;

                float tentativeGScore = gScore[current] + Heuristic(current.transform.position, neighbor.transform.position);

                if (!openSet.Contains(neighbor))
                    openSet.Add(neighbor);
                else if (tentativeGScore >= gScore[current])
                    continue;

                cameFrom[neighbor] = current;
                gScore[neighbor] = tentativeGScore;
                fScore[neighbor] = gScore[neighbor] + Heuristic(neighbor.transform.position, goal.transform.position);
            }
        }

        return null; // No path found
    }

    private static List<TeleportPoint> ReconstructPath(Dictionary<TeleportPoint, TeleportPoint> cameFrom, TeleportPoint current)
    {
        List<TeleportPoint> totalPath = new List<TeleportPoint> { current };
        while (cameFrom.ContainsKey(current))   // スタートに戻るまでループ
        {
            current = cameFrom[current];
            totalPath.Add(current);
        }
        totalPath.Reverse();
        return totalPath;
    }

    public static void VisualizePathForDebug(List<TeleportPoint> path)
    {
        if (path == null || path.Count == 0)
            return;

        for (int i = 0; i < path.Count - 1; i++)
        {
            Debug.DrawLine(path[i].transform.position, path[i + 1].transform.position, Color.red, 5f);
        }
    }
}
