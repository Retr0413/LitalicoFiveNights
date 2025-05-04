using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class MapController : MonoBehaviour
{
    [Header("階層ごとのオブジェクトリスト")]
    public List<GameObject> groundObjects;
    public List<GameObject> firstFloorObjects;
    public List<GameObject> secondFloorObjects;

    [Header("階層切り替えボタン")]
    public Button groundButton;
    public Button firstFloorButton;
    public Button secondFloorButton;

    void Start()
    {
        // ボタンにイベント登録
        if (groundButton != null) groundButton.onClick.AddListener(() => ShowOnly(groundObjects));
        if (firstFloorButton != null) firstFloorButton.onClick.AddListener(() => ShowOnly(firstFloorObjects));
        if (secondFloorButton != null) secondFloorButton.onClick.AddListener(() => ShowOnly(secondFloorObjects));

        // 最初にGroundを表示する場合
        ShowOnly(groundObjects);
    }

    void ShowOnly(List<GameObject> targetList)
    {
        // すべてのオブジェクトを非表示
        SetActiveList(groundObjects, false);
        SetActiveList(firstFloorObjects, false);
        SetActiveList(secondFloorObjects, false);

        // 対象のリストのみ表示
        SetActiveList(targetList, true);
    }

    void SetActiveList(List<GameObject> list, bool isActive)
    {
        foreach (GameObject obj in list)
        {
            if (obj != null)
                obj.SetActive(isActive);
        }
    }
}
