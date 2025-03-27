using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    public float detectionRange = 2.0f;
    public float attackCooldown = 1.5f; // 攻撃間隔
    private Animator animator;
    private bool isAttacking = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);
        bool isPlayerNear = distance < detectionRange;

        animator.SetBool("isPlayerNear", isPlayerNear);

        // プレイヤーが近く、攻撃していない場合のみ攻撃を開始
        if (isPlayerNear && !isAttacking)
        {
            StartCoroutine(AttackLoop());
        }
    }

    private IEnumerator AttackLoop()
    {
        isAttacking = true;

        while (true)
        {
            float distance = Vector3.Distance(transform.position, player.position);
            if (distance >= detectionRange)
            {
                isAttacking = false;
                yield break; // 攻撃ループを終了
            }

            // 確実にアニメーションを再生
            animator.CrossFade("Punch", 0.1f);

            // 少し待ってからアニメーションの長さを取得
            yield return new WaitForSeconds(0.1f);
            float attackAnimLength = animator.GetCurrentAnimatorStateInfo(0).length;

            // もし取得した長さが 0 の場合、デフォルト値を設定
            if (attackAnimLength == 0)
            {
                attackAnimLength = 1.0f; // 1秒のデフォルト値（適宜調整）
            }

            // アニメーションが終わるまで待機
            yield return new WaitForSeconds(attackAnimLength);

            // ダメージ処理
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRange);
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.CompareTag("Player"))
                {
                    PlayerHealth playerHealth = hitCollider.GetComponent<PlayerHealth>();
                    if (playerHealth != null)
                    {
                        playerHealth.TakeDamage(1);
                    }
                }
            }

            // クールダウンを待機
            yield return new WaitForSeconds(attackCooldown);
            
            // 攻撃の状態をリセット
            if (distance >= detectionRange)
            {
                isAttacking = false;
                yield break; // 攻撃ループを終了
            }
        }
    }
}
