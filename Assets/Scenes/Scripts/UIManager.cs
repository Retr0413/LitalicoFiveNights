using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject panel;         // 操作用パネル
    public Button[] controlButtons;  // パネル内のボタン（カメラ移動や壁ボタン）
    public CameraManager cameraManager; // CameraManager参照

    private void Start()
    {
        panel.SetActive(false);
        SetControlButtonsActive(false);
    }

    public void TogglePanel()
    {
        bool isActive = !panel.activeSelf;
        panel.SetActive(isActive);
        SetControlButtonsActive(isActive);

        if (!isActive)
        {
            cameraManager.ReturnToMainCamera(); // パネル閉じたらメインカメラに戻る
        }
    }

    private void SetControlButtonsActive(bool isActive)
    {
        foreach (Button button in controlButtons)
        {
            button.gameObject.SetActive(isActive);
        }
    }
}
