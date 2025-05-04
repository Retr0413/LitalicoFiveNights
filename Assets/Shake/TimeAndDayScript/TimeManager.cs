using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Collections;

public class TimeManager : MonoBehaviour
{
    public Text timeText;
    public Image timeGauge;
    public int startHour = 12;
    public int endHour = 6;
    public float hourInterval = 20f;

    public static event Action<int> OnDayChanged;
    public static event Action OnGameClear;  // ★ Clear時イベント追加

    private int currentHour;
    private float timer = 0f;
    private int totalHours;
    private float totalTime;
    private int currentDay = 1;
    private bool hasCleared = false;

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
        if (hasCleared) return;

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
                    hasCleared = true;
                    UpdateTimeText();
                    OnGameClear?.Invoke(); // ★ Clearイベント発火
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
