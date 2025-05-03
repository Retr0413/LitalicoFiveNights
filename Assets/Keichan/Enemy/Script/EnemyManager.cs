using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private EnemyArray[] enemyList = new EnemyArray[5]; // 敵のリストを格納する2次元配列

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        TimeManager.OnDayChanged += StartDay; // TimeManagerからのイベントを受け取る
    }

    void StartDay(int day)
    {
        Debug.Log($"Day {day} enemy spawn started."); // 日付が変わったときの処理
        if (day > 0 && day <= enemyList.Length) // dayが1以上enemyListの長さ以下のとき
        {
            foreach (var enemy in enemyList[day - 1].enemys) // 敵の配列を取得
            {
                enemy.SpawnEnemy();
            }
        }
    }
}

[System.Serializable]
class EnemyArray
{
    [SerializeField] public EnemyData[] enemys;
    // [SerializeField] public TeleportPoint playerPosition; // 敵の名前を格納する変数
    // [SerializeField] public EnemyMove[] enemyArray; // 敵の配列を格納するクラス
    // [SerializeField] public StartPoints[] startPoints; // スタート地点を格納するクラス]

    // public void SpawnEnemy()
    // {
    //     for (int i = 0; i < enemyArray.Length; i++)
    //     {
    //         GameObject enemyObj = GameObject.Instantiate(enemyArray[i].gameObject);
    //         EnemyMove enemyMove = enemyObj.GetComponent<EnemyMove>();
    //         enemyMove.playerPosition = playerPosition; // プレイヤーの位置を設定する処理
    //         enemyMove.startPoints = startPoints[i].startPoints; // スタート地点を設定する処理
    //         enemyMove.gameObject.SetActive(true); // 敵をアクティブにする処理
    //     }
    // }
}

[System.Serializable]
public class StartPoints
{
    public TeleportPoint[] startPoints; // スタート地点を格納するクラス
}