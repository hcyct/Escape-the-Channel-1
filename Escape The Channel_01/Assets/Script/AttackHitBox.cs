using UnityEngine;

public class AttackHitBox : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy")) // 确保敌人有 "Enemy" 标签
        {
            Debug.Log("检测到敌人");
            EnemyController enemy = other.GetComponent<EnemyController>();
            if (enemy != null)
            {
                enemy.TakeDamage(); // 调用敌人的受伤方法
            }
        }
    }
}
