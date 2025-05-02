using UnityEngine;
using UnityEngine.UI;
using System;

public class TimeManager : MonoBehaviour
{
    public Text timeText;            // 現在の時間を表示
    public Image timeGauge;          // ゲージ表示
    public int startHour = 12;       // 開始時間（固定）
    public int endHour = 6;          // 終了時間（固定）
    public float hourInterval = 20f; // 1時間の経過秒数

    public static event Action<int> OnDayChanged;

    private int currentHour;
    private float timer = 0f;
    private int totalHours;
    private float totalTime;
    private int currentDay = 1;

    void Start()
    {
        currentHour = startHour;
        totalHours = (endHour + 12) - startHour; // 6 - 12 + 12 = 6
        totalTime = hourInterval * totalHours;

        UpdateTimeText();
        OnDayChanged?.Invoke(currentDay);
    }

    void Update()
    {
        // Day6超えたらストップ（Clear扱い）
        if (currentDay > 5) return;

        timer += Time.deltaTime;

        if (timer >= hourInterval)
        {
            timer -= hourInterval;
            currentHour++;

            if (currentHour > 12) currentHour = 1; // 12 → 1 → 2 → ...

            UpdateTimeText();

            // 日が変わる条件（6時になったら）
            if (currentHour == endHour)
            {
                currentDay++;

                if (currentDay <= 5)
                {
                    currentHour = startHour;
                    timer = 0f;

                    if (timeGauge != null)
                        timeGauge.fillAmount = 1f;

                    OnDayChanged?.Invoke(currentDay);
                    UpdateTimeText();
                }
                else
                {
                    // 最終日超えた後
                    UpdateTimeText();
                }
            }
        }

        // ゲージ減少ロジック（経過時間から算出）
        if (timeGauge != null)
        {
            int hoursPassed = (currentHour >= startHour)
                ? currentHour - startHour
                : (12 - startHour + currentHour); // 例：12→1→2→3 のときの経過時間

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
