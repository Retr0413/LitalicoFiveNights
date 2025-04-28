using UnityEngine;
using UnityEngine.UI;  // ← SliderとImageを使うので必要

public class SystemManager : MonoBehaviour
{
    public GameObject DoorManager;
    public GameObject PlayerButton;

    [Header("バッテリーバーとイメージリスト")]
    public GameObject[] batteryBars;     // バッテリーバー (例: Sliderオブジェクト)
    public GameObject[] batteryImages;   // バッテリーイメージ (例: Imageオブジェクト)

    private bool isDoorActive = false; // DoorManagerがオンになっているかどうか

    void Start()
    {
        if (DoorManager != null)
            DoorManager.SetActive(false);

        if (PlayerButton != null)
            PlayerButton.SetActive(true);

        // 初期状態: 最初のバーとイメージだけONにして、他はOFF
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
        // ① 左クリックでDoorManagerをオンにする
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

                    // クリック時: バッテリーの表示切り替え
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

            // Eキー押したら: バッテリーの表示を元に戻す
            SwitchBatteryUI(toSecond: false);
        }
    }

    // 👇 バッテリーバーとイメージの切り替えメソッド
    private void SwitchBatteryUI(bool toSecond)
    {
        if (batteryBars.Length >= 2 && batteryImages.Length >= 2)
        {
            if (toSecond)
            {
                // 2番目をON、1番目をOFF
                batteryBars[0].SetActive(false);
                batteryImages[0].SetActive(false);
                batteryBars[1].SetActive(true);
                batteryImages[1].SetActive(true);
            }
            else
            {
                // 1番目をON、2番目をOFF
                batteryBars[0].SetActive(true);
                batteryImages[0].SetActive(true);
                batteryBars[1].SetActive(false);
                batteryImages[1].SetActive(false);
            }
        }
    }
}
