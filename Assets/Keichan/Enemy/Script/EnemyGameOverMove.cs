using UnityEngine;
using System.Collections;

public class EnemyGameOverMove : MonoBehaviour
{
    private Animator animator;
    public GameOverPanel gameOverPanel;
    public float attackTime;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(AttackStart());
    }

    IEnumerator AttackStart()
    {
        yield return new WaitForSeconds(1f); // 0.5秒待機
        animator.SetTrigger("Attack"); // トリガーを発火
        if(attackTime != null)
        {
            yield return new WaitForSeconds(attackTime); // 攻撃時間を待機
            AttackEnd();
        }
        yield break;
    }

    public void AttackEnd()
    {
        Debug.Log("AttackEnd");
        gameOverPanel.ShowGameOverPanel();
    }
}
