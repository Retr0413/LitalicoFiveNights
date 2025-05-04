using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class DayTransitionUI : MonoBehaviour
{
    public Text dayText;
    public Image blackBackground;
    public float fadeDuration = 1f;
    public float displayDuration = 2f;

    public List<GameObject> otherUIElements;

    private void OnEnable()
    {
        TimeManager.OnDayChanged += ShowDayTransition;
        TimeManager.OnGameClear += StartGameClearSequence; // ★ Clear時イベント登録
    }

    private void OnDisable()
    {
        TimeManager.OnDayChanged -= ShowDayTransition;
        TimeManager.OnGameClear -= StartGameClearSequence;
    }

    public void ShowDayTransition(int newDay)
    {
        StartCoroutine(FadeRoutine(newDay));
    }

    private IEnumerator FadeRoutine(int day)
    {
        foreach (GameObject ui in otherUIElements)
            if (ui != null) ui.SetActive(false);

        dayText.gameObject.SetActive(true);
        blackBackground.gameObject.SetActive(true);

        dayText.color = new Color(1, 1, 1, 0);
        blackBackground.color = new Color(0, 0, 0, 0);

        dayText.text = $"Day {day - 1} → Day {day}";

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

        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            float alpha = 1f - (t / fadeDuration);
            dayText.color = new Color(1, 1, 1, alpha);
            blackBackground.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        dayText.gameObject.SetActive(false);
        blackBackground.gameObject.SetActive(false);

        if (day <= 6)
        {
            foreach (GameObject ui in otherUIElements)
                if (ui != null) ui.SetActive(true);
        }
    }

    // ★ Clear遷移演出
    private void StartGameClearSequence()
    {
        StartCoroutine(GameClearRoutine());
    }

    private IEnumerator GameClearRoutine()
    {
        blackBackground.gameObject.SetActive(true);
        Color color = blackBackground.color;
        color.a = 0;
        blackBackground.color = color;

        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            float alpha = t / fadeDuration;
            color.a = alpha;
            blackBackground.color = color;
            yield return null;
        }

        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("GameClear");
    }
}
