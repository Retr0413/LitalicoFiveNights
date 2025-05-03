using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class NoiseCameraEffect : MonoBehaviour
{
    [Header("対象カメラのリスト")]
    public List<Camera> cameras;

    [Header("現在有効なカメラのインデックス")]
    public int activeCameraIndex = 0;

    [Header("ノイズ画像（透明PNG）")]
    public Sprite noiseSprite;

    [Header("ノイズ透明度範囲")]
    public float minAlpha = 0.1f;
    public float maxAlpha = 0.4f;

    [Header("ノイズ変化速度")]
    public float noiseSpeed = 2.0f;

    private Canvas noiseCanvas;
    private Image noiseImage;

    void Start()
    {
        CreateNoiseCanvas();
        UpdateCameraState(); // 最初に1つ目のカメラを有効に
    }

    void Update()
    {
        if (noiseCanvas.gameObject.activeSelf)
        {
            float noiseBase = Mathf.PerlinNoise(Time.time * noiseSpeed, 0);
            
            // 👇 ノイズ倍率を補正する
            if (CameraShowEnemy.instance != null && CameraShowEnemy.instance.isEnemyVisible)
            {
                // ノイズを強調（maxAlpha に近づける）
                noiseBase = Mathf.Clamp01(noiseBase + 0.5f);
            }

            float alpha = Mathf.Lerp(minAlpha, maxAlpha, noiseBase);

            Color color = noiseImage.color;
            color.a = alpha;
            noiseImage.color = color;
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            activeCameraIndex = (activeCameraIndex + 1) % cameras.Count;
            UpdateCameraState();
        }
    }

    private void CreateNoiseCanvas()
    {
        GameObject canvasObj = new GameObject("NoiseCanvas");
        noiseCanvas = canvasObj.AddComponent<Canvas>();
        noiseCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        noiseCanvas.sortingOrder = -1; // UIより背後に表示

        CanvasScaler scaler = canvasObj.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;

        // Imageの作成
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

    private void UpdateCameraState()
    {
        for (int i = 0; i < cameras.Count; i++)
        {
            cameras[i].enabled = (i == activeCameraIndex);
        }

        // ノイズ表示は特定カメラのときだけ（例：Index 0 のときだけ）
        noiseCanvas.gameObject.SetActive(activeCameraIndex == 0); // ← ここで制御
    }
}
