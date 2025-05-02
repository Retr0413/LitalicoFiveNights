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
    [SerializeField] private float preTeleportTime = 1f;

    [SerializeField] private bool preTeleport = false;

    private Coroutine moveCoroutine; // プレイヤーに向かうコルーチン

    void Start () {
        ResetPosition(); // スタート地点をランダムに決める
        moveCoroutine = StartCoroutine(MoveToPlayer()); // プレイヤーに向かう
    }

    private void Update(){
        if (current.IsNearestDoorClosed()){
            Rerouting();
        }
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

    private void Rerouting(){
        StopMoveCoroutine();
        ResetPosition();
        moveCoroutine = StartCoroutine(MoveToPlayer());
    }

    private void StopMoveCoroutine(){
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
            moveCoroutine = null;
        }
    }

    IEnumerator MoveToPlayer()
    {
        Debug.Log("Moving to player...");
        List<TeleportPoint> route = getRoute();
        if (route == null || route.Count == 0){
            Rerouting();
            yield break;
        }

        foreach (var point in route)
        {
            float waitTime = Random.Range(minWaitTime, maxWaitTime);
            yield return new WaitForSeconds(waitTime);
            preTeleport = true;
            yield return new WaitForSeconds(preTeleportTime);
            preTeleport = false;
            SetPosition(point);
        }
    }
}