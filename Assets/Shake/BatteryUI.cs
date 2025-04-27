using UnityEngine;
using UnityEngine.UI;

public class BatteryUI : MonoBehaviour
{
    [Header("バッテリーUI要素")]
    public Text BatteryText;
    public Slider BatterySlider;

    [Header("バッテリーの設定")]
    [Range(0, 100)]
    public float BatteryPerventage = 100f;
    public float DrainSpeed = 2f;

    public WallBlocker BlockCheck;

    public DoorMove DoorCheck;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateBatteryUI();
    }

    // Update is called once per frame
    void Update()
    {
        if (BatteryPerventage > 0 && BlockCheck.Block)
        {
            BatteryPerventage -= DrainSpeed * Time.deltaTime;
            UpdateBatteryUI();
        }
        else if (BatteryPerventage <= 0)
        {
            BatteryPerventage = 0;
            UpdateBatteryUI();
        }

        if (BatteryPerventage > 0 && DoorCheck.Lock)
        {
            BatteryPerventage -= 5f;
            UpdateBatteryUI();
        }
    }

    void UpdateBatteryUI()
    {
        if (BatteryText != null)
        {
            BatteryText.text = "Battery: " + Mathf.RoundToInt(BatteryPerventage) + "%";
        }
        if (BatterySlider != null)
        {
            BatterySlider.value = BatteryPerventage / 100f;
        }
    }
}
