using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public Text healthText; // 绑定 UI 组件

    public float runSpeed = 5f;
    public float jumpForce = 10f;

    public LayerMask groundLayer; // 地面层
    public Transform groundCheck; // 地面检测点
    public float groundCheckRadius = 0.2f; // 地面检测范围
    public GameObject attackHitbox; // 攻击碰撞体
    public float attcakTiming = 0.15f;

    private Rigidbody2D rb;
    private Animator animator;
    private Collider2D normalCollider;
    private Collider2D slideCollider;
    private bool isGrounded;
    private bool isSliding;
    private bool isJumping;
    private bool isFalling;
    private bool isAttacking;
    private bool isKnockedBack;


    public int playerHealth = 3; // 玩家生命值
    public float knockbackForce = 5f; // 受伤击退力度
    public float invincibleTime = 0.2f; // 无敌时间
    public float knockBackTiming = 0.5f;
    private bool isInvincible;

    public GameObject gameOverPanel;
    void Start()
    {
        UpdateHealthUI(); // 确保游戏开始时 UI 显示正确的血量
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        normalCollider = GetComponent<Collider2D>();
        slideCollider = transform.Find("SlideCollider").GetComponent<Collider2D>();

        slideCollider.enabled = false;
        attackHitbox.SetActive(false); // 初始关闭攻击区域
    }

    void Update()
    {
        if (!isKnockedBack)
        {
            rb.velocity = new Vector2(runSpeed, rb.velocity.y);
        }

        if (transform.position.y < -8)
        {
            Respawn();
        }

        isGrounded = CheckIfGrounded();

        //  跳跃：空格 或 手柄 A
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.JoystickButton0))
            && isGrounded && !isSliding)
        {
            Jump();
        }

        //  滑铲：S 键 或 手柄 B（持续按住）
        if ((Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.JoystickButton1)) && isGrounded)
        {
            StartSlide();
        }
        else if (Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.JoystickButton1))
        {
            StopSlide();
        }

        // 攻击：鼠标左键（你也可以扩展为手柄X）
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.JoystickButton2)) // 支持手柄X键
        {
            Attack();
        }


        UpdateJumpAnimation();
    }

    void UpdateHealthUI()
    {
        if (healthText != null)
        {
            healthText.text = "Health: " + playerHealth;
        }
    }

    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        isJumping = true;
        isFalling = false;
        animator.SetBool("IsJumping", true);
        animator.SetBool("IsFalling", false);
    }

    void StartSlide()
    {
        if (!isSliding)
        {
            isSliding = true;
            animator.SetBool("IsSliding", true);

            // 切换碰撞体
            normalCollider.enabled = false;
            slideCollider.enabled = true;
        }
    }

    void StopSlide()
    {
        if (isSliding)
        {
            isSliding = false;
            animator.SetBool("IsSliding", false);

            // 还原碰撞体
            normalCollider.enabled = true;
            slideCollider.enabled = false;
        }
    }

    void Attack()
    {
        if (!isAttacking)
        {
            isAttacking = true;
            animator.SetTrigger("Attack"); // 播放攻击动画
            attackHitbox.SetActive(true); // 启用攻击碰撞体

            // 一段时间后关闭攻击碰撞体
            StartCoroutine(ResetAttack());
        }
    }

    IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(attcakTiming); // 假设攻击动画持续0.5秒
        attackHitbox.SetActive(false);
        isAttacking = false;

        // 根据当前状态恢复动画
        if (isGrounded)
        {
            animator.SetBool("IsRunning", true);
        }
        else
        {
            if (rb.velocity.y > 0)
            {
                animator.SetBool("IsJumping", true);
                animator.SetBool("IsFalling", false);
            }
            else
            {
                animator.SetBool("IsJumping", false);
                animator.SetBool("IsFalling", true);
            }
        }
    }

    void UpdateJumpAnimation()
    {
        if (!isGrounded)
        {
            if (rb.velocity.y > 0) // 向上跳跃
            {
                isJumping = true;
                isFalling = false;
                animator.SetBool("IsJumping", true);
                animator.SetBool("IsFalling", false);
            }
            else if (rb.velocity.y < 0) // 下落
            {
                isJumping = false;
                isFalling = true;
                animator.SetBool("IsJumping", false);
                animator.SetBool("IsFalling", true);
            }
        }
        else // 角色接触地面
        {
            if (isFalling) // 只有在 Falling 状态时才切换
            {
                isJumping = false;
                isFalling = false;
                animator.SetBool("IsJumping", false);
                animator.SetBool("IsFalling", false);
            }
        }
    }

    bool CheckIfGrounded()
    {
        // 在地面检测点发射一个小范围的射线来检测地面
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && !isInvincible)
        {
            TakeDamage(collision.gameObject.transform);
        }
    }
    void TakeDamage(Transform enemy)
    {
        if (isInvincible) return; // 防止重复受伤

        playerHealth--; // 生命值减少
        playerHealth = Mathf.Max(playerHealth, 0);
        animator.SetTrigger("Hurt"); // 播放受伤动画
        UpdateHealthUI(); // 更新 UI
        if (playerHealth <= 0)
        {
            Die();
            return;
        }

        StartCoroutine(BecomeInvincible());

        float knockbackDirection = transform.position.x > enemy.position.x ? 1 : -1;
        rb.velocity = new Vector2(knockbackDirection * knockbackForce, 3f);
        isKnockedBack = true;
        StartCoroutine(ResetKnockback());


        // 取消滑铲状态，防止玩家滑铲时受伤出现异常
        if (isSliding)
        {
            StopSlide();
        }

        // 短暂停止跑步
        StartCoroutine(StopRunningTemporarily());
    }


    void Die()
    {
        animator.SetTrigger("Die");
        StartCoroutine(GameOverPanel());
        this.enabled = false; // 禁用 PlayerController
        rb.velocity = Vector2.zero; // 清除速度
    }

    IEnumerator BecomeInvincible()
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibleTime);
        isInvincible = false;
    }
    IEnumerator StopRunningTemporarily()
    {
        float originalSpeed = runSpeed;
        runSpeed = 0; // 停止前进

        yield return new WaitForSeconds(1f); // 受击后停顿0.3秒

        runSpeed = originalSpeed; // 恢复速度
    }
    IEnumerator ResetKnockback()
    {
        yield return new WaitForSeconds(knockBackTiming); // 击退持续时间
        isKnockedBack = false;
    }
    void Respawn()
    {
        playerHealth--; // 生命值减少
        playerHealth = Mathf.Max(playerHealth, 0);
        UpdateHealthUI(); // 更新 UI
        if (playerHealth <= 0)
        {
            Die();
            return;
        }
        
        // 复位到 X 位置不变，但 Y 轴设为 8
        transform.position = new Vector3(transform.position.x, 8f, transform.position.z);
    }
     IEnumerator GameOverPanel()
    {
        yield return new WaitForSeconds(1f);
        gameOverPanel.SetActive(true);
    }
    public void BackToMenu()
    {
        SceneManager.LoadScene("StartScene");
    }
}
