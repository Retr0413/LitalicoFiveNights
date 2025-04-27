using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public float mouseSensitivity = 100f; // マウス感度
    public Transform playerBody;          // プレイヤー本体（カメラの親オブジェクト）

    private float xRotation = 0f;

    void Start()
    {
        // カーソルをロックして画面中央に固定
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // マウスの動きを取得
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // 上下方向の回転を計算
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // 上を向きすぎたり、後ろを向きすぎないように制限

        // カメラ（上下）の回転を適用
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // プレイヤー本体（左右）の回転を適用
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
