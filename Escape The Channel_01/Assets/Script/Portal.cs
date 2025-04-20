using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    public string nextSceneName; // Ŀ�곡������

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // ȷ������ҽ���
        {
            SceneManager.LoadScene(nextSceneName); // ������һ������
        }
    }
}
