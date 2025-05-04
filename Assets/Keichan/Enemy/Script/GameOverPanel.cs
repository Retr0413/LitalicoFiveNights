using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverPanel : MonoBehaviour
{
    private Animator animator;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("StartScene"); // タイトルシーンに遷移
    }

    public void ShowGameOverPanel()
    {
        animator.SetTrigger("GameOver"); // トリガーを発火
    }
}
