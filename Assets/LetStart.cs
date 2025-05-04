using UnityEngine;
using UnityEngine.SceneManagement; // ← シーン遷移に必要

public class LetStart : MonoBehaviour
{
    // ボタンから呼び出すための関数
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
