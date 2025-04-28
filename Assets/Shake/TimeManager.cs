using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TimeManager : MonoBehaviour
{
    public Text timeText;            // 現在の時間を表示するText
    public Image timeGauge;           // 円形ゲージ
    public int startHour = 12;        // 開始時間（例：12時）
    public int endHour = 6;           // 終了時間（例：6時）
    public float hourInterval = 20f;  // 1時間経過するまでの秒数（例：20秒）

    private int currentHour;
    private float timer = 0f;
    private int totalHours;
    private float totalTime;

    void Start()
    {
        currentHour = startHour;
        UpdateTimeText();

        // 12時から6時までに何時間進むか計算（6時間）
        totalHours = (endHour + 12) - startHour; // 6 - 12 + 12 = 6
        totalTime = hourInterval * totalHours;
    }

    void Update()
    {
        timer += Time.deltaTime;

        // 時間更新
        if (timer >= hourInterval)
        {
            timer -= hourInterval;
            currentHour++;

            // 12時を超えたら1時にする処理
            if (currentHour == 13) currentHour = 1;

            UpdateTimeText();
        }

        // ゲージ更新
        if (timeGauge != null)
        {
            float elapsed = (Time.timeSinceLevelLoad) / totalTime;
            timeGauge.fillAmount = Mathf.Clamp01(1f - elapsed);
        }
    }

    private void UpdateTimeText()
    {
        if (timeText != null)
        {
            timeText.text = currentHour + "時";
        }
    }
}
