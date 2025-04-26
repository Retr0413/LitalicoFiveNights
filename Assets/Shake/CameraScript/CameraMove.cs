using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public float rotationAmount = 45f;           // 回転する角度（例：45度）
    public GameObject leftButton;                // 左ボタン
    public GameObject rightButton;               // 右ボタン

    private Quaternion originalRotation;         // 初期角度（正面）
    private int currentView = 0;                 // -1: 左, 0: 正面, 1: 右
    private float rotationSpeed = 5f;            // 回転スピード

    void Start()
    {
        originalRotation = transform.rotation;
    }

    void Update()
    {
        // 左キーが押された時
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (currentView == 0)
            {
                currentView = -1;
                RotateToAngle(-rotationAmount);
                leftButton.SetActive(false);  // 左を見たら左ボタン非表示
                rightButton.SetActive(true);
            }
            else if (currentView == 1)
            {
                currentView = 0;
                RotateToAngle(0);
                leftButton.SetActive(true);   // 正面に戻ったら再び表示
                rightButton.SetActive(true);
            }
        }

        // 右キーが押された時
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (currentView == 0)
            {
                currentView = 1;
                RotateToAngle(rotationAmount);
                rightButton.SetActive(false); // 右を見たら右ボタン非表示
                leftButton.SetActive(true);
            }
            else if (currentView == -1)
            {
                currentView = 0;
                RotateToAngle(0);
                rightButton.SetActive(true);  // 正面に戻ったら再び表示
                leftButton.SetActive(true);
            }
        }
    }

    public void RotateToAngle(float yAngle)
    {
        Quaternion targetRotation = Quaternion.Euler(0, yAngle, 0);
        StopAllCoroutines();
        StartCoroutine(SmoothRotate(targetRotation));
    }

    System.Collections.IEnumerator SmoothRotate(Quaternion target)
    {
        while (Quaternion.Angle(transform.rotation, target) > 0.1f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, target, rotationSpeed * Time.deltaTime);
            yield return null;
        }
        transform.rotation = target; // 最終調整
    }
}
