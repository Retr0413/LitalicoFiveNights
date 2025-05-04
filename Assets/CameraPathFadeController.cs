using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // ← シーン遷移に必要
using System.Collections;
using System.Collections.Generic;

public class CameraPathFadeController : MonoBehaviour
{
    [Header("カメラ")]
    public Transform cameraTransform;

    [Header("カメラが移動する目的地リスト（順番）")]
    public List<Transform> pathPoints;

    [Header("移動設定")]
    public float moveDuration = 2f;

    [Header("フェード設定")]
    public GameObject fadeImageObject;
    private Image fadeImage;

    public float fadeDuration = 2f;

    void Start()
    {
        if (fadeImageObject != null)
        {
            fadeImageObject.SetActive(false);
            fadeImage = fadeImageObject.GetComponent<Image>();
        }

        StartCoroutine(MoveAlongPath());
    }

    IEnumerator MoveAlongPath()
    {
        for (int i = 0; i < pathPoints.Count; i++)
        {
            Transform target = pathPoints[i];
            yield return StartCoroutine(MoveCameraToTarget(target));
        }

        yield return new WaitForSeconds(1f);

        if (fadeImageObject != null)
            fadeImageObject.SetActive(true);

        yield return StartCoroutine(FadeOut());
    }

    IEnumerator MoveCameraToTarget(Transform target)
    {
        Vector3 start = cameraTransform.position;
        Vector3 end = target.position;
        float elapsed = 0f;

        while (elapsed < moveDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / moveDuration);
            cameraTransform.position = Vector3.Lerp(start, end, t);
            yield return null;
        }
    }

    IEnumerator FadeOut()
    {
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsed / fadeDuration);

            if (fadeImage != null)
            {
                Color color = fadeImage.color;
                color.a = alpha;
                fadeImage.color = color;
            }

            yield return null;
        }

        Debug.Log("フェードアウト完了");

        // ★ フェード完了後にシーン遷移
        SceneManager.LoadScene("StartScene");
    }
}
