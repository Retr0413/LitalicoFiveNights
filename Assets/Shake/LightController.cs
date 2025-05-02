using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LightController : MonoBehaviour
{
    [Header("対象のライトオブジェクト")]
    public List<Light> lights = new List<Light>();

    [Header("点滅の最小/最大間隔")]
    public float minInterval = 1.0f;
    public float maxInterval = 5.0f;

    [Header("点滅の回数範囲")]
    public int minBlinkCount = 1;
    public int maxBlinkCount = 5;

    [Header("点滅の間隔（秒）")]
    public float blinkDuration = 0.1f;

    void Start()
    {
        foreach (var light in lights)
        {
            StartCoroutine(HandleLight(light));
        }
    }

    IEnumerator HandleLight(Light light)
    {
        while (true)
        {
            // ランダムな待機時間
            float waitTime = Random.Range(minInterval, maxInterval);
            yield return new WaitForSeconds(waitTime);

            // ランダムに動作を選択
            int action = Random.Range(0, 3); // 0:OFF, 1:ON, 2:点滅

            switch (action)
            {
                case 0:
                    light.enabled = false;
                    break;
                case 1:
                    light.enabled = true;
                    break;
                case 2:
                    int blinkCount = Random.Range(minBlinkCount, maxBlinkCount + 1);
                    for (int i = 0; i < blinkCount; i++)
                    {
                        light.enabled = false;
                        yield return new WaitForSeconds(blinkDuration);
                        light.enabled = true;
                        yield return new WaitForSeconds(blinkDuration);
                    }
                    break;
            }
        }
    }
}
