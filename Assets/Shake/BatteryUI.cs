using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class BatteryUI : MonoBehaviour
{
    [Header("バッテリーUI要素")]
    public List<Text> batteryTexts = new List<Text>();     // ← Textをリスト化
    public List<Slider> batterySliders = new List<Slider>(); // ← Sliderをリスト化

    [Header("バッテリーの設定")]
    [Range(0, 100)]
    public float BatteryPercentage = 100f;
    public float drainPerSecondDoor = 5f;    // ドアでの減少（%/秒）
    public float drainPerSecondWall = 5f;    // 壁での減少（%/秒）

    [Header("チェック対象")]
    public List<MonoBehaviour> targetComponents = new List<MonoBehaviour>();

    private List<IDoor> doors = new List<IDoor>();
    private List<IWallBlocker> walls = new List<IWallBlocker>();

    private void Start()
    {
        UpdateBatteryUI();

        foreach (var component in targetComponents)
        {
            if (component is IDoor door)
            {
                doors.Add(door);
            }
            if (component is IWallBlocker wall)
            {
                walls.Add(wall);
            }
        }
    }

    private void Update()
    {
        float totalDrain = 0f;

        foreach (var door in doors)
        {
            if (door.Lock)
            {
                totalDrain += drainPerSecondDoor * Time.deltaTime;
            }
        }

        foreach (var wall in walls)
        {
            if (wall.Block)
            {
                totalDrain += drainPerSecondWall * Time.deltaTime;
            }
        }

        if (BatteryPercentage > 0)
        {
            BatteryPercentage -= totalDrain;
            if (BatteryPercentage < 0)
            {
                BatteryPercentage = 0;
            }
            UpdateBatteryUI();
        }

        DebugLogDoorWallStates();
    }

    private void UpdateBatteryUI()
    {
        foreach (var text in batteryTexts)
        {
            if (text != null)
            {
                text.text = "Battery: " + Mathf.RoundToInt(BatteryPercentage) + "%";
            }
        }

        foreach (var slider in batterySliders)
        {
            if (slider != null)
            {
                slider.value = BatteryPercentage / 100f;
            }
        }
    }

    private void DebugLogDoorWallStates()
    {
        for (int i = 0; i < doors.Count; i++)
        {
            Debug.Log($"[Door {i}] Lock状態: {doors[i].Lock}");
        }

        for (int i = 0; i < walls.Count; i++)
        {
            Debug.Log($"[Wall {i}] Block状態: {walls[i].Block}");
        }
    }
}
