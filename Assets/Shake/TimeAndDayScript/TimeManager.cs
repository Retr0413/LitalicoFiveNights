using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // ← シーン遷移に必要
using System;

public class TimeManager : MonoBehaviour
{
    public Text timeText;
    public Image timeGauge;
    public int startHour = 12;
    public int endHour = 6;
    public float hourInterval = 20f;

    public static event Action<int> OnDayChanged;

    private int currentHour;
    private float timer = 0f;
    private int totalHours;
    private float totalTime;
    private int currentDay = 1;

    void Start()
    {
        currentHour = startHour;
        totalHours = (endHour + 12) - startHour;
        totalTime = hourInterval * totalHours;

        UpdateTimeText();
        OnDayChanged?.Invoke(currentDay);
        Debug.Log($"[DayChanged Start] Day {currentDay}");
    }

    void Update()
    {
        // Day6超えたらストップ（Clear扱い）＋シーン遷移
        if (currentDay > 5)
        {
            Debug.Log("6日目終了。シーン遷移します。");
            SceneManager.LoadScene("GameClear"); // ← シーン名を必要に応じて変更
            return;
        }

        timer += Time.deltaTime;

        if (timer >= hourInterval)
        {
            timer -= hourInterval;
            currentHour++;
            if (currentHour > 12) currentHour = 1;

            if (currentHour == endHour)
            {
                currentDay++;

                if (currentDay <= 5)
                {
                    currentHour = startHour;
                    timer = 0f;

                    if (timeGauge != null)
                        timeGauge.fillAmount = 1f;

                    UpdateTimeText();
                    OnDayChanged?.Invoke(currentDay);
                    Debug.Log($"[DayChanged] Day {currentDay}");
                    return;
                }
                else
                {
                    UpdateTimeText(); // 表示を"Clear!"に更新
                    return;
                }
            }

            UpdateTimeText();
        }

        if (timeGauge != null)
        {
            int hoursPassed = (currentHour >= startHour)
                ? currentHour - startHour
                : (12 - startHour + currentHour);

            float totalElapsed = (hoursPassed * hourInterval) + timer;
            float remaining = Mathf.Clamp01(1f - (totalElapsed / totalTime));
            timeGauge.fillAmount = remaining;
        }
    }

    private void UpdateTimeText()
    {
        if (timeText != null)
        {
            if (currentDay <= 5)
                timeText.text = $"Day{currentDay} - {currentHour}時";
            else
                timeText.text = "Clear!";
        }
    }
}
