using UnityEngine;

public class AttackHitBox : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy")) // ȷ�������� "Enemy" ��ǩ
        {
            Debug.Log("��⵽����");
            EnemyController enemy = other.GetComponent<EnemyController>();
            if (enemy != null)
            {
                enemy.TakeDamage(); // ���õ��˵����˷���
            }
        }
    }
}
