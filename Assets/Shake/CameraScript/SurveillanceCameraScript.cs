using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SurveillanceCameraScript : MonoBehaviour
{
    [Header("監視カメラ（RenderTexture出力あり）")]
    public List<Camera> surveillanceCameras;

    [Header("対応するUIのImageリスト")]
    public List<Image> cameraUIImages;

    [Header("プレイヤーの視点カメラ")]
    public Camera mainCamera;

    [Header("次・戻るボタン")]
    public Button nextButton;
    public Button backButton;

    [Header("カメラ名を表示するText（監視カメラ視点時）")]
    public Text cameraLabelText;

    [Header("MainCamera時にカメラ名を表示するText")]
    public Text mainCameraInfoText;

    [Header("SystemManagerの参照")]
    public SystemManager systemManager;

    [Header("プレイヤー用UI（監視モード時に非表示）")]
    public GameObject playerUI;

    [Header("監視カメラ表示用テキストオブジェクト（ON/OFF切り替え対象）")]
    public GameObject cameraTextObject;

    [Header("ノイズ画像（透明PNG）")]
    public Sprite noiseSprite;

    [Header("ノイズ透明度範囲")]
    public float minAlpha = 0.1f;
    public float maxAlpha = 0.4f;

    [Header("ノイズ変化速度")]
    public float noiseSpeed = 2.0f;

    private int currentCameraIndex = 0;
    private bool isInSurveillanceMode = true;

    private Vector3 mainCameraDefaultPosition;
    private Quaternion mainCameraDefaultRotation;

    private Canvas noiseCanvas;
    private Image noiseImage;

    void Start()
    {
        if (mainCamera != null)
        {
            mainCameraDefaultPosition = mainCamera.transform.position;
            mainCameraDefaultRotation = mainCamera.transform.rotation;
        }

        if (surveillanceCameras.Count > 0)
        {
            SwitchToCamera(currentCameraIndex);
        }

        if (nextButton != null) nextButton.onClick.AddListener(NextCamera);
        if (backButton != null) backButton.onClick.AddListener(PreviousCamera);

        CreateNoiseCanvas();
        noiseCanvas.gameObject.SetActive(false);

        isInSurveillanceMode = false;
        ReturnToMainCameraDefault();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit) && hit.collider.CompareTag("Monitor"))
            {
                if (systemManager != null && systemManager.IsFacingMonitor())
                {
                    isInSurveillanceMode = true;
                    SwitchToCamera(currentCameraIndex);
                }
                else
                {
                    Debug.Log("プレイヤーが正面（Y=180±20）を向いていないため、カメラ切り替え不可。");
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Q) && isInSurveillanceMode)
        {
            isInSurveillanceMode = false;
            ReturnToMainCameraDefault();
        }

        UpdateUIVisibility();

        if (isInSurveillanceMode && noiseCanvas != null)
        {
            float alpha = Mathf.Lerp(minAlpha, maxAlpha, Mathf.PerlinNoise(Time.time * noiseSpeed, 0));
            Color color = noiseImage.color;
            color.a = alpha;
            noiseImage.color = color;
        }
    }

    void SwitchToCamera(int index)
    {
        if (index >= 0 && index < surveillanceCameras.Count)
        {
            currentCameraIndex = index;

            mainCamera.transform.position = surveillanceCameras[index].transform.position;
            mainCamera.transform.rotation = surveillanceCameras[index].transform.rotation;

            if (cameraLabelText != null)
                cameraLabelText.text = $"現在のカメラ: {surveillanceCameras[index].name}";

            if (mainCameraInfoText != null)
                mainCameraInfoText.text = "";

            if (cameraTextObject != null)
                cameraTextObject.SetActive(true);

            if (playerUI != null)
                playerUI.SetActive(false);

            if (noiseCanvas != null)
                noiseCanvas.gameObject.SetActive(true);

            UpdateCameraUIColor(); // 🔴 修正ポイント：ここで色を更新

            Debug.Log($"[監視カメラ切替] Index: {index}, Name: {surveillanceCameras[index].name}");
        }
    }

    void UpdateCameraUIColor()
    {
        for (int i = 0; i < cameraUIImages.Count; i++)
        {
            if (cameraUIImages[i] != null)
            {
                cameraUIImages[i].color = (i == currentCameraIndex) ? Color.red : Color.white;
            }
        }
    }

    void NextCamera()
    {
        currentCameraIndex = (currentCameraIndex + 1) % surveillanceCameras.Count;
        SwitchToCamera(currentCameraIndex);
    }

    void PreviousCamera()
    {
        currentCameraIndex = (currentCameraIndex - 1 + surveillanceCameras.Count) % surveillanceCameras.Count;
        SwitchToCamera(currentCameraIndex);
    }

    void ReturnToMainCameraDefault()
    {
        mainCamera.transform.position = mainCameraDefaultPosition;
        mainCamera.transform.rotation = mainCameraDefaultRotation;

        if (cameraLabelText != null)
            cameraLabelText.text = "";

        if (cameraTextObject != null)
            cameraTextObject.SetActive(false);

        if (mainCameraInfoText != null && surveillanceCameras.Count > 0)
            mainCameraInfoText.text = $"現在表示中のカメラ映像: {surveillanceCameras[currentCameraIndex].name}";

        if (playerUI != null)
            playerUI.SetActive(true);

        if (noiseCanvas != null)
            noiseCanvas.gameObject.SetActive(false);

        UpdateCameraUIColor(); // 🔴 戻っても赤を維持

        Debug.Log("[監視カメラ解除] メインカメラに戻りました");
    }

    void UpdateUIVisibility()
    {
        if (nextButton != null) nextButton.gameObject.SetActive(isInSurveillanceMode);
        if (backButton != null) backButton.gameObject.SetActive(isInSurveillanceMode);
    }

    void CreateNoiseCanvas()
    {
        GameObject canvasObj = new GameObject("NoiseCanvas");
        noiseCanvas = canvasObj.AddComponent<Canvas>();
        noiseCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        noiseCanvas.sortingOrder = -1;

        CanvasScaler scaler = canvasObj.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;

        GameObject noiseObj = new GameObject("NoiseImage");
        noiseObj.transform.SetParent(canvasObj.transform, false);
        noiseImage = noiseObj.AddComponent<Image>();
        noiseImage.sprite = noiseSprite;
        noiseImage.rectTransform.anchorMin = Vector2.zero;
        noiseImage.rectTransform.anchorMax = Vector2.one;
        noiseImage.rectTransform.offsetMin = Vector2.zero;
        noiseImage.rectTransform.offsetMax = Vector2.zero;
        noiseImage.color = new Color(1, 1, 1, minAlpha);
    }

    public Camera GetCurrentCamera()
    {
        if (currentCameraIndex >= 0 && currentCameraIndex < surveillanceCameras.Count)
            return surveillanceCameras[currentCameraIndex];
        return null;
    }

    public int GetCurrentCameraIndex()
    {
        return currentCameraIndex;
    }
}
