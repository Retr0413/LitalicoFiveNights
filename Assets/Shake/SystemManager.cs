using UnityEngine;
using UnityEngine.UI;

public class SystemManager : MonoBehaviour
{
    public GameObject DoorManager;
    public GameObject PlayerButton;

    [Header("バッテリーバーとイメージリスト")]
    public GameObject[] batteryBars;
    public GameObject[] batteryImages;

    [Header("プレイヤー参照")]
    public Transform playerTransform;  // ← プレイヤー（またはMainCamera）のTransformを指定

    private bool isDoorActive = false;

    void Start()
    {
        if (DoorManager != null) DoorManager.SetActive(false);
        if (PlayerButton != null) PlayerButton.SetActive(true);

        if (batteryBars.Length >= 2 && batteryImages.Length >= 2)
        {
            batteryBars[0].SetActive(true);
            batteryImages[0].SetActive(true);
            batteryBars[1].SetActive(false);
            batteryImages[1].SetActive(false);
        }
    }

    void Update()
    {
        // ① 左クリックでDoorManagerをオンにする（正面を向いている場合のみ）
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("Panel"))
                {
                    isDoorActive = true;

                    if (DoorManager != null)
                        DoorManager.SetActive(true);

                    if (PlayerButton != null)
                        PlayerButton.SetActive(false);

                    SwitchBatteryUI(toSecond: true);
                }
            }
        }

        // ② EキーでDoorManagerをオフにする
        if (isDoorActive && Input.GetKeyDown(KeyCode.E))
        {
            isDoorActive = false;

            if (DoorManager != null)
                DoorManager.SetActive(false);

            if (PlayerButton != null)
                PlayerButton.SetActive(true);

            SwitchBatteryUI(toSecond: false);
        }
    }

    private void SwitchBatteryUI(bool toSecond)
    {
        if (batteryBars.Length >= 2 && batteryImages.Length >= 2)
        {
            batteryBars[0].SetActive(!toSecond);
            batteryImages[0].SetActive(!toSecond);
            batteryBars[1].SetActive(toSecond);
            batteryImages[1].SetActive(toSecond);
        }
    }

    // 正面がY=180度（±20度の範囲）に向いているか
    public bool IsFacingMonitor()
    {
        if (playerTransform == null) return false;

        float y = playerTransform.eulerAngles.y;
        return (y >= 160f && y <= 200f);  // Y=180 ±20の範囲
    }
}
