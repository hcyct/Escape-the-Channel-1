using UnityEngine;
using UnityEngine.UI;

public class Coin : MonoBehaviour
{
    public Text coinCountText; // UI��Ӳ��������ʾ�ı�
    public AudioClip coinSound; // Ӳ����Ч
    private static int coinCount = 0; // Ӳ������

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // ������Ч
            PlayCoinSound();

            // ����Ӳ������
            coinCount++;

            // ����UI����ʾ��Ӳ������
            if (coinCountText != null)
            {
                coinCountText.text = "Coins: " + coinCount.ToString();
            }

            // ����Ӳ������
            Destroy(gameObject);
        }
    }

    // ����Ӳ����Ч�ķ���
    private void PlayCoinSound()
    {
        if (coinSound != null)
        {
            AudioSource.PlayClipAtPoint(coinSound, transform.position);
        }
    }
}
