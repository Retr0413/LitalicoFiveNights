using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SurveillanceCameraScript : MonoBehaviour
{
    [Header("監視カメラ（RenderTexture出力あり）")]
    public List<Camera> surveillanceCameras;

    [Header("プレイヤーの視点カメラ")]
    public Camera mainCamera;

    [Header("次・戻るボタン")]
    public Button nextButton;
    public Button backButton;

    [Header("カメラ名を表示するText（監視カメラ視点時）")]
    public Text cameraLabelText;

    [Header("MainCamera時にカメラ名を表示するText")]
    public Text mainCameraInfoText;

    private int currentCameraIndex = 0;
    private bool isInSurveillanceMode = true;

    private Vector3 mainCameraDefaultPosition;
    private Quaternion mainCameraDefaultRotation;

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
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("Monitor"))
                {
                    isInSurveillanceMode = true;
                    SwitchToCamera(currentCameraIndex);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            isInSurveillanceMode = false;
            ReturnToMainCameraDefault();
        }

        UpdateUIVisibility();
    }

    void SwitchToCamera(int index)
    {
        if (index >= 0 && index < surveillanceCameras.Count)
        {
            mainCamera.transform.position = surveillanceCameras[index].transform.position;
            mainCamera.transform.rotation = surveillanceCameras[index].transform.rotation;

            if (cameraLabelText != null)
            {
                cameraLabelText.text = $"現在のカメラ: {surveillanceCameras[index].name}";
            }

            if (mainCameraInfoText != null)
            {
                mainCameraInfoText.text = ""; // MainCameraの時のみ表示する
            }

            Debug.Log($"[監視カメラ切替] Index: {index}, Name: {surveillanceCameras[index].name}");
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
        {
            cameraLabelText.text = "";
        }

        if (mainCameraInfoText != null && surveillanceCameras.Count > 0)
        {
            mainCameraInfoText.text = $"現在表示中のカメラ映像: {surveillanceCameras[currentCameraIndex].name}";
        }

        Debug.Log("[監視カメラ解除] メインカメラに戻りました");
    }

    void UpdateUIVisibility()
    {
        if (nextButton != null) nextButton.gameObject.SetActive(isInSurveillanceMode);
        if (backButton != null) backButton.gameObject.SetActive(isInSurveillanceMode);
    }

    // 外部用 Getter
    public Camera GetCurrentCamera()
    {
        if (currentCameraIndex >= 0 && currentCameraIndex < surveillanceCameras.Count)
        {
            return surveillanceCameras[currentCameraIndex];
        }
        return null;
    }

    public int GetCurrentCameraIndex()
    {
        return currentCameraIndex;
    }
}
