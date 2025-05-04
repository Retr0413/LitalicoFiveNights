using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;

public class EnemyMove : MonoBehaviour {

    public TeleportPoint playerPosition; 
    [SerializeField] private TeleportPoint current;
    public TeleportPoint[] startPoints; // この中からランダムに決める

    [SerializeField] private float minWaitTime = 0.5f;
    [SerializeField] private float maxWaitTime = 3.0f; // 移動速度
    [SerializeField] private float preTeleportTime = 1f;

    [SerializeField] private bool preTeleport = false;

    public string GameOverSceneName = "GameOverScene";

    private Coroutine moveCoroutine; // プレイヤーに向かうコルーチン。止めるために必要

    void Start () {
        TimeManager.OnDayChanged += EndDay; // 日付が変わったときの処理
        ResetPosition(); // スタート地点をランダムに決める
        moveCoroutine = StartCoroutine(MoveToPlayer()); // プレイヤーに向かう
    }

    private void Update(){
        // 近くの障害物が閉じていたら、初期値に戻ってルートを再計算
        if (current.IsNearestObstaclesClosed()){
            Rerouting();
        }
    }

    private void Rerouting(){
        StopMoveCoroutine();
        ResetPosition();
        foreach(var point in startPoints){
            Debug.Log(point.gameObject.name);
        }
        moveCoroutine = StartCoroutine(MoveToPlayer());
    }

    private void ReachPlayer(){
        StopMoveCoroutine();
        SceneManager.LoadScene(GameOverSceneName);
    }

    public void EndDay(int day){
        StopMoveCoroutine();
        Destroy(gameObject);
    }

    private List<TeleportPoint> getRoute(){
        
        List<TeleportPoint> route = Pathfinder.GetRandomPath(current, playerPosition);
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
            if(point == playerPosition){
                ReachPlayer();
                yield break;
            }
            SetPosition(point);
        }
    }
}