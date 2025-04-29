using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class NowCameraScript : MonoBehaviour
{
    [Header("RawImageに表示する対象")]
    public RawImage rawImage;

    [Header("参照する監視カメラスクリプト")]
    public SurveillanceCameraScript cameraController; // 前回作成したスクリプトをアタッチ

    private int currentIndex = -1;

    void Update()
    {
        // 現在表示中のカメラIndexを取得
        int activeIndex = cameraController.GetCurrentCameraIndex();

        // Indexが変わったら、RawImageのテクスチャを更新
        if (activeIndex != currentIndex)
        {
            currentIndex = activeIndex;
            UpdateRawImageTexture();
        }
    }

    void UpdateRawImageTexture()
    {
        Camera currentCamera = cameraController.GetCurrentCamera();
        if (currentCamera != null && currentCamera.targetTexture != null)
        {
            rawImage.texture = currentCamera.targetTexture;
        }
        else
        {
            rawImage.texture = null;
        }
    }
}
