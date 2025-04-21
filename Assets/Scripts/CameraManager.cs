using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Camera mainCamera;       // メインカメラ
    public Camera[] cameras;        // 切り替えカメラ
    private int currentCameraIndex = 0;

    private void Start()
    {
        if (cameras.Length > 0)
        {
            ActivateCamera(0);
        }
    }

    public void NextCamera()
    {
        currentCameraIndex = (currentCameraIndex + 1) % cameras.Length;
        ActivateCamera(currentCameraIndex);
    }

    public void PreviousCamera()
    {
        currentCameraIndex = (currentCameraIndex - 1 + cameras.Length) % cameras.Length;
        ActivateCamera(currentCameraIndex);
    }

    void ActivateCamera(int index)
    {
        mainCamera.gameObject.SetActive(false); // MainCameraは無効にする
        for (int i = 0; i < cameras.Length; i++)
        {
            cameras[i].gameObject.SetActive(i == index);
        }
    }

    public void ReturnToMainCamera()
    {
        for (int i = 0; i < cameras.Length; i++)
        {
            cameras[i].gameObject.SetActive(false);
        }
        mainCamera.gameObject.SetActive(true);
    }
}
