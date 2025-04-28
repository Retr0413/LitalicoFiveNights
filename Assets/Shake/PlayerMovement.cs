using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // 移動速度を設定できるように

    void Update()
    {
        // 入力を取得
        float moveX = Input.GetAxis("Horizontal"); // A,Dキーまたは←→キー
        float moveZ = Input.GetAxis("Vertical");   // W,Sキーまたは↑↓キー

        // 移動ベクトルを作成
        Vector3 move = new Vector3(moveX, 0, moveZ);

        // 移動
        transform.Translate(move * moveSpeed * Time.deltaTime);
    }
}
