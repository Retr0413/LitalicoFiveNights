using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using System.Collections;

public class MoveTo : MonoBehaviour {

    [SerializeField] private TeleportPoint playerPosition; 
    [SerializeField] private TeleportPoint current;
    [SerializeField] private TeleportPoint[] startPoints; // この中からランダムに決める

    [SerializeField] private Pathfinder pathfinder; // 経路探索

    [SerializeField] private float minWaitTime = 0.5f;
    [SerializeField] private float maxWaitTime = 3.0f; // 移動速度

    void Start () {
        ResetPosition(); // スタート地点をランダムに決める
        StartCoroutine(MoveToPlayer()); // プレイヤーに向かう
    }

    private void ReachPlayer(){

    }

    private List<TeleportPoint> getRoute(){
        
        List<TeleportPoint> route = Pathfinder.AStar(current, playerPosition);
        if (route == null || route.Count == 0)
        {
            Debug.Log("No path found");
            return null;
        }

        foreach (var point in route)
        {
            Debug.Log(point.name);
        }

        return route;
    }

    private void SetPosition(TeleportPoint point)
    {
        transform.position = point.transform.position;
        current = point;
    }

    private void ResetPosition()
    {
        TeleportPoint start = startPoints[Random.Range(0, startPoints.Length)];
        SetPosition(start);
    }

    IEnumerator MoveToPlayer()
    {
        List<TeleportPoint> route = getRoute();
        if (route == null || route.Count == 0)
            yield break;

        foreach (var point in route)
        {
            float waitTime = Random.Range(minWaitTime, maxWaitTime);
            yield return new WaitForSeconds(waitTime);
            SetPosition(point);
        }
    }
}