using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CameraMove : MonoBehaviour
{
    public float rotationAmount = 40f;         // 1回押すごとの回転角度
    public GameObject leftButton;              // 左ボタン (GameObject)
    public GameObject rightButton;             // 右ボタン (GameObject)
    public Button leftUIButton;                // 左ボタン (UI.Button)
    public Button rightUIButton;               // 右ボタン (UI.Button)
    public GameObject leftLight;
    public GameObject rightLight;

    private float rotationSpeed = 5f;           // 回転スピード
    private float currentAngle = 0f;            // 現在のカメラ角度
    private int currentView = 0;                // -1:左 0:正面 1:右

    void Start()
    {
        currentAngle = transform.eulerAngles.y;

        leftLight.SetActive(false);
        rightLight.SetActive(false);

        // ボタンクリック時に呼び出すメソッドを設定
        leftUIButton.onClick.AddListener(OnLeftButtonClick);
        rightUIButton.onClick.AddListener(OnRightButtonClick);

        UpdateButtonVisibility();
    }

    void Update()
    {
        // スペースキーで正面に戻す
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (currentView != 0) // 正面以外の時だけ
            {
                currentView = 0; // 正面にリセット
                leftLight.SetActive(false);
                rightLight.SetActive(false);
                RotateToFront();
                UpdateButtonVisibility();
            }
        }
    }

    public void OnLeftButtonClick()
    {
        if (currentView > -1)
        {
            currentView -= 1;
            leftLight.SetActive(true);
            RotateRelative(-rotationAmount);
            UpdateButtonVisibility();
        }
    }

    public void OnRightButtonClick()
    {
        if (currentView < 1)
        {
            currentView += 1;
            rightLight.SetActive(true);
            RotateRelative(rotationAmount);
            UpdateButtonVisibility();
        }
    }

    public void RotateRelative(float addYAngle)
    {
        currentAngle += addYAngle;
        Quaternion targetRotation = Quaternion.Euler(0, currentAngle, 0);
        StopAllCoroutines();
        StartCoroutine(SmoothRotate(targetRotation));
    }

    public void RotateToFront()
    {
        currentAngle = 180f; // 正面の角度
        Quaternion targetRotation = Quaternion.Euler(0, currentAngle, 0);
        StopAllCoroutines();
        StartCoroutine(SmoothRotate(targetRotation));
    }

    IEnumerator SmoothRotate(Quaternion target)
    {
        while (Quaternion.Angle(transform.rotation, target) > 0.1f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, target, rotationSpeed * Time.deltaTime);
            yield return null;
        }
        transform.rotation = target;
    }

    private void UpdateButtonVisibility()
    {
        if (currentView == 0)
        {
            // 正面に戻ったら両方ON
            leftButton.SetActive(true);
            rightButton.SetActive(true);
        }
        else
        {
            // 左端なら左ボタンを非表示
            leftButton.SetActive(currentView > -1);
            // 右端なら右ボタンを非表示
            rightButton.SetActive(currentView < 1);
        }
    }
}
