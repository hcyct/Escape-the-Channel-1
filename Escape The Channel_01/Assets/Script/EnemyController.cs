using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float moveSpeed = 2f;

    private Transform targetPoint;
    private SpriteRenderer spriteRenderer;
    private Collider2D enemyCollider;
    private Animator animator;
    private bool isDead = false;

    void Start()
    {
        targetPoint = pointB;
        spriteRenderer = GetComponent<SpriteRenderer>();
        enemyCollider = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!isDead)
        {
            Patrol();
        }
    }

    void Patrol()
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPoint.position, moveSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, targetPoint.position) < 0.1f)
        {
            targetPoint = (targetPoint == pointA) ? pointB : pointA;
            Flip();
        }
    }

    void Flip()
    {
        Vector3 newScale = transform.localScale;
        newScale.x *= -1; // 反转X轴方向
        transform.localScale = newScale;
    }

    public void TakeDamage()
    {
        Debug.Log("TakeDamage调用");
        if (!isDead)
        {
            isDead = true;
            animator.SetTrigger("Die");
            enemyCollider.enabled = false; // 取消碰撞体

            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = Vector2.zero; // 清除当前速度，避免累积影响
                rb.AddForce(new Vector2(20f, 4f), ForceMode2D.Impulse); // 施加一个向右上方的力
            }
        }
    }

}
