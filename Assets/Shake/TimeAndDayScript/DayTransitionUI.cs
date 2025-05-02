using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class DayTransitionUI : MonoBehaviour
{
    public Text dayText;                    // 表示用テキスト
    public Image blackBackground;           // 黒背景用
    public float fadeDuration = 1f;         // フェード時間
    public float displayDuration = 2f;      // 表示時間

    public List<GameObject> otherUIElements; // その他のUI（隠す対象）

    private void OnEnable()
    {
        TimeManager.OnDayChanged += ShowDayTransition;
    }

    private void OnDisable()
    {
        TimeManager.OnDayChanged -= ShowDayTransition;
    }

    public void ShowDayTransition(int newDay)
    {
        StartCoroutine(FadeRoutine(newDay));
    }

    private IEnumerator FadeRoutine(int day)
    {
        // 1. UI初期化・非表示化
        foreach (GameObject ui in otherUIElements)
        {
            if (ui != null) ui.SetActive(false);
        }

        dayText.gameObject.SetActive(true);
        blackBackground.gameObject.SetActive(true);

        dayText.color = new Color(1, 1, 1, 0);
        blackBackground.color = new Color(0, 0, 0, 0);

        // 2. テキスト設定（Day7以降は "Clear!"）
        if (day > 6)
        {
            dayText.text = "Clear!";
        }
        else
        {
            dayText.text = $"Day {day - 1} → Day {day}";
        }

        // 3. フェードイン
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            float alpha = t / fadeDuration;
            dayText.color = new Color(1, 1, 1, alpha);
            blackBackground.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        dayText.color = new Color(1, 1, 1, 1);
        blackBackground.color = new Color(0, 0, 0, 1);

        yield return new WaitForSeconds(displayDuration);

        // 4. フェードアウト
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            float alpha = 1f - (t / fadeDuration);
            dayText.color = new Color(1, 1, 1, alpha);
            blackBackground.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        // 5. 終了処理
        dayText.gameObject.SetActive(false);
        blackBackground.gameObject.SetActive(false);

        // Day6までなら他UIを再表示
        if (day <= 6)
        {
            foreach (GameObject ui in otherUIElements)
            {
                if (ui != null) ui.SetActive(true);
            }
        }
    }
}
