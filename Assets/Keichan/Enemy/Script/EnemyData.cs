using UnityEngine;

[System.Serializable]
public class EnemyData : MonoBehaviour
{
    public TeleportPoint playerPoint;
    public EnemyMove enemy;
    public TeleportPoint[] startPoints;

    public void SpawnEnemy()
    {
        GameObject enemyObj = GameObject.Instantiate(enemy.gameObject);
        EnemyMove enemyMove = enemyObj.GetComponent<EnemyMove>();
        enemyMove.playerPosition = playerPoint; // プレイヤーの位置を設定する処理
        enemyMove.startPoints = startPoints; // スタート地点を設定する処理
        enemyMove.gameObject.SetActive(true); // 敵をアクティブにする処理
    }
}
